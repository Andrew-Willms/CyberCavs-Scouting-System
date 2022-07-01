using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using WPFUtilities.Extensions;

namespace WPFUtilities.Validation;



public interface IMultiInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }
}

public interface IMultiInput<out TTargetType, TSeverityEnum> : IInput<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public class MultiInput<TTargetType, TSeverityEnum> : Input<TTargetType, TSeverityEnum>, IMultiInput<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TTargetType? _TargetObject;
	public override TTargetType? TargetObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _TargetObject : default;

		protected set {
			_TargetObject = value;
			OnTargetObjectChanged();
		}
	}

	public ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> StringInputs { get; }

	public override ValidationEvent TargetObjectChanged { get; } = new();

	private MultiInputConverter<TTargetType?, TSeverityEnum> Converter { get; }
	private ReadOnlyList<MultiInputValidator<TTargetType, TSeverityEnum>> DefaultValidators { get; }
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	private ReadOnlyList<ValidationError<TSeverityEnum>> ComponentErrors => StringInputs.Values.SelectMany(x => x.Errors).ToReadOnly();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRanges(ValidationErrors, ComponentErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ComponentErrorLevel => ComponentErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, ValidationErrorLevel, ComponentErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;



	public MultiInput(MultiInputConverter<TTargetType?, TSeverityEnum> converter,
		ReadOnlyList<(string inputComponentName, IStringInput<TSeverityEnum> stringInput)> inputComponents,
		ReadOnlyList<MultiInputValidator<TTargetType, TSeverityEnum>> defaultValidators,
		params IValidationSet<TTargetType, TSeverityEnum>[] validationSets) {

		Converter = converter;
		DefaultValidators = defaultValidators;
		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		StringInputs = new(inputComponents.ToDictionary(x => x.inputComponentName, x => x.stringInput));

		foreach (IStringInput<TSeverityEnum> inputString in StringInputs.Values) {
			inputString.TargetObjectChanged.Subscribe(Validate);
		}

		Validate();
	}


	public sealed override void Validate() {

		(TargetObject, ConversionErrors) = Converter(StringInputs);

		ValidationErrors.Clear();

		if (TargetObject is not null) {

			foreach (MultiInputValidator<TTargetType, TSeverityEnum> covalidator in DefaultValidators) {
				ValidationErrors.AddIfNotNull(covalidator.Invoke(TargetObject));
			}

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddIfNotNull(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetObject)));
		TargetObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}