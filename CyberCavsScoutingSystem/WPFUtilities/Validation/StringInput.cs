using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities.Validation;



public interface IStringInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public string InputString { get; set; }
}

public interface IStringInput<out TTargetType, TSeverityEnum> : IInput<TTargetType, TSeverityEnum>, IStringInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public class StringInput<TTargetType, TSeverityEnum> : Input<TTargetType, TSeverityEnum>, IStringInput<TTargetType, TSeverityEnum>
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

	private StringInputConverter<TTargetType, TSeverityEnum> Converter { get; }
	private ReadOnlyList<StringInputValidator<TTargetType, TSeverityEnum>> DefaultValidators { get; }
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRange(ValidationErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, ValidationErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;

	public override ValidationEvent TargetObjectChanged { get; } = new();


	public StringInput(StringInputConverter<TTargetType, TSeverityEnum> converter, string initialString,
		ReadOnlyList<StringInputValidator<TTargetType, TSeverityEnum>> defaultValidators,
		params IValidationSet<TTargetType, TSeverityEnum>[] validationSets) {

		Converter = converter;
		DefaultValidators = defaultValidators;
		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		InputString = initialString;
	}



	public sealed override void Validate() {

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



	public override event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetObject)));
		TargetObjectChanged.Invoke();
	}

	private void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}