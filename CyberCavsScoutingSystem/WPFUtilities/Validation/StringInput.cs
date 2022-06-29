using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities.Validation;



public interface IStringInput<TSeverityEnum> : INotifyPropertyChanged
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public string InputString { get; set; }

	public bool IsValid { get; }

	public ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }

	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
	public TSeverityEnum ErrorLevel { get; }

	public void Validate();

}



public class StringInput<TTargetType, TSeverityEnum> : IStringInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _TargetObject : default;

		private set {
			_TargetObject = value;
			OnTargetObjectChanged();
		}
	}

	private string _InputString = "";
	public string InputString {

		// TODO: .Net 7.0 remove backing field
		get => _InputString;

		set {
			_InputString = value;
			OnInputStringChanged();
			Validate();
		}
	}

	private ValidationEvent TargetObjectChanged { get; } = new();
	private ReadOnlyList<ValidationEvent> EventsToInvolve { get; }

	private StringInputConverter<TTargetType, TSeverityEnum> Converter { get; }
	private ReadOnlyList<StringInputValidator<TTargetType, TSeverityEnum>> DefaultValidators { get; }
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	private List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	public ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRange(ValidationErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, ValidationErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public bool IsValid => ErrorLevel.IsFatal == false;



	public StringInput(StringInputConverter<TTargetType, TSeverityEnum> converter, string initialString,
		ReadOnlyList<ValidationEvent> eventsToInvoke, ReadOnlyList<StringInputValidator<TTargetType, TSeverityEnum>> defaultValidators,
		params IValidationSet<TTargetType, TSeverityEnum>[] validationSets) {

		Converter = converter;
		DefaultValidators = defaultValidators;
		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		EventsToInvolve = eventsToInvoke;

		InputString = initialString;
	}

	// Try moving this up a level of inheritance
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TTargetType, TSeverityEnum>> validationSets) {

		TTargetType TargetObjectGetter() {

			if (TargetObject is null) {
				throw new NullReferenceException($"Validators should not be called if {nameof(TargetObject)} is null.");
			}

			return TargetObject;
		}

		return validationSets.Select(x => x.ToValidationTrigger(TargetObjectGetter, PostValidation)).ToReadOnly();
	}



	public void Validate() {

		(TargetObject, ConversionErrors) = Converter(InputString);

		ValidationErrors.Clear();

		if (TargetObject is not null) {

			foreach (StringInputValidator<TTargetType, TSeverityEnum> validator in DefaultValidators) {
				ValidationErrors.AddIfNotNull(validator.Invoke(TargetObject));
			}

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddIfNotNull(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}

	private void PostValidation(ValidationError<TSeverityEnum> validationError) {

		ValidationErrors.Add(validationError);

		OnErrorsChanged();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetObject)));

		TargetObjectChanged.Invoke();
		foreach (ValidationEvent validationEvent in EventsToInvolve) {
			validationEvent.Invoke();
		}
	}

	private void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	private void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}