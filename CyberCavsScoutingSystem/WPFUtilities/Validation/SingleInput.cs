using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities.Validation;



public interface ISingleInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> { }

public interface ISingleInput<TInput, TSeverityEnum> : IInput<TSeverityEnum>, ISingleInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public TInput InputObject { get; set; }
}

public interface ISingleInput<out TOutput, TInput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>, ISingleInput<TInput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public class SingleInput<TOutput, TInput, TSeverityEnum> : Input<TOutput, TSeverityEnum>, ISingleInput<TOutput, TInput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _TargetObject;
	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _TargetObject : default;

		protected set {

			//(TTargetType result, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Converter(InputObject);

			//if (errors.Any() == false && result == )

			_TargetObject = value;
			OnTargetObjectChanged();
		}
	}

	private TInput _InputObject;
	public TInput InputObject {

		// TODO: .Net 7.0 remove backing field
		get => _InputObject;

		set {
			_InputObject = value;
			OnInputChanged();
			Validate();
		}
	}

	private SingleInputConverter<TOutput?, TInput, TSeverityEnum> Converter { get; }
	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRange(ValidationErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, ValidationErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;

	public override ValidationEvent OutputObjectChanged { get; } = new();



	//public SingleInput(SingleInputConverter<TOutput?, TInput, TSeverityEnum> converter) : this(converter, default) { }

	public SingleInput(SingleInputConverter<TOutput?, TInput, TSeverityEnum> converter, TInput initialInput,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets) {

		Converter = converter;
		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		_InputObject = initialInput;
		InputObject = initialInput;
	}



	public sealed override void Validate() {

		(OutputObject, ConversionErrors) = Converter(InputObject);

		ValidationErrors.Clear();

		if (OutputObject is not null) {

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddRange(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	private void OnInputChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputObject)));
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}