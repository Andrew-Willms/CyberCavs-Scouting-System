using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Configuration;

namespace WPFUtilities.Validation;



public interface IMultiInput<TSeverityEnum> : INotifyPropertyChanged
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public bool IsValid { get; }

	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }

	public ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }

	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
	public TSeverityEnum ErrorLevel { get; }

	public void Validate();

}

public class MultiInput<TTargetType, TSeverityEnum> : IMultiInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _TargetObject : default;

		private set {
			_TargetObject = value;
			Validate();
			OnTargetObjectChanged();
		}
	}

	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }

	private ValidationEvent TargetObjectChanged { get; } = new();
	private ReadOnlyList<ValidationEvent> EventsToInvolve { get; }

	private MultiInputConverter<TTargetType, TSeverityEnum> Converter { get; }
	private ReadOnlyList<MultiInputCovalidator<TTargetType, TSeverityEnum>> DefaultCovalidators { get; }
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> CovalidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	private List<ValidationError<TSeverityEnum>> CovalidationErrors { get; } = new();
	private ReadOnlyList<ValidationError<TSeverityEnum>> ComponentErrors => StringInputs.Values.SelectMany(x => x.Errors).ToReadOnly();
	public ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRanges(CovalidationErrors, ComponentErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum CovalidationErrorLevel => CovalidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ComponentErrorLevel => ComponentErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, CovalidationErrorLevel, ComponentErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public bool IsValid => ErrorLevel.IsFatal == false;



	public MultiInput(MultiInputConverter<TTargetType, TSeverityEnum> converter,
		ReadOnlyList<(string inputComponentName, IStringInput<TSeverityEnum> stringInput)> inputComponents,
		ReadOnlyList<ValidationEvent> eventsToInvoke,
		ReadOnlyList<MultiInputCovalidator<TTargetType, TSeverityEnum>> defaultCovalidators,
		params IValidationSet<TTargetType, TSeverityEnum>[] validationSets) {

		Converter = converter;
		DefaultCovalidators = defaultCovalidators;
		CovalidationTriggers = ValidationSetsToTriggers(validationSets);

		EventsToInvolve = eventsToInvoke;

		StringInputs = new(inputComponents.ToDictionary(x => x.inputComponentName, x => x.stringInput));

		foreach (IStringInput<TSeverityEnum> inputString in StringInputs.Values) {
			inputString.PropertyChanged += OnComponentInputChanged;
		}

		Validate();
	}

	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TTargetType, TSeverityEnum>> validationSets) {

		TTargetType TargetObjectGetter() {

			if (TargetObject is null) {
				throw new NullReferenceException($"Validators should not be called if {nameof(TargetObject)} is null.");
			}

			return TargetObject;
		}

		IEnumerable<IValidationTrigger<TSeverityEnum>> validationTriggers = validationSets
			.Select(validationSet => validationSet.ToValidationTrigger(TargetObjectGetter, PostValidation));

		return validationTriggers.ToReadOnly();
	}



	private void OnComponentInputChanged(object? sender, PropertyChangedEventArgs e) {
		Validate();
	}

	public void Validate() {

		(TargetObject, ConversionErrors) = Converter(StringInputs);

		CovalidationErrors.Clear();

		if (TargetObject is not null) {

			foreach (MultiInputCovalidator<TTargetType, TSeverityEnum> covalidator in DefaultCovalidators) {
				CovalidationErrors.AddIfNotNull(covalidator.Invoke(TargetObject));
			}

			foreach (IValidationTrigger<TSeverityEnum> trigger in CovalidationTriggers) {
				CovalidationErrors.AddIfNotNull(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}

	private void PostValidation(ValidationError<TSeverityEnum> validationError) {

		CovalidationErrors.Add(validationError);

		OnErrorsChanged();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetObject)));
	}

	private void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}