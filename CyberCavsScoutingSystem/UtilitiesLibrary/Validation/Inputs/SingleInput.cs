using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Inputs;



public interface ISingleInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> { }

public interface ISingleInput<TInput, TSeverityEnum> : IInput<TSeverityEnum>, ISingleInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public TInput InputObject { get; set; }
}

public interface ISingleInput<TOutput, TInput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>, ISingleInput<TInput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public class SingleInput<TOutput, TInput, TSeverityEnum> : Input<TOutput, TSeverityEnum>, ISingleInput<TOutput, TInput, TSeverityEnum>
	where TInput : IEquatable<TInput>
	where TOutput : IEquatable<TOutput>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _OutputObject;
	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : default;

		set {

			// I feel like this shouldn't be here
			if (value is null) {
				throw new InvalidOperationException($"You cannot set {nameof(OutputObject)} to a null value.");
			}

			if (value.Equals(_OutputObject)) {
				//OnOutputObjectChanged();
				return;
			}

			(Optional<TInput> inversionResult, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

			if (errors.Any(x => x.Severity.IsFatal) || !inversionResult.HasValue) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to a value that cannot be inverted.");
			}

			if (!inversionResult.Value.Equals(InputObject)) {
				InputObject = inversionResult.Value;
			}

			_OutputObject = value;
			OnOutputObjectChanged();
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

	private InputConverterErrorList<TOutput, TInput, TSeverityEnum> Converter { get; }
	private InputInverterErrorList<TOutput, TInput, TSeverityEnum> Inverter { get; }

	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRange(ValidationErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Operations.Max(ConversionErrorLevel, ValidationErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;

	public override ValidationEvent OutputObjectChanged { get; } = new();



	public SingleInput(ConversionPair<TOutput, TInput, TSeverityEnum> conversionPair, TInput initialInput,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)
		: this(conversionPair.Converter, conversionPair.Inverter, initialInput, validationSets) { }

	public SingleInput(InputConverterErrorList<TOutput, TInput, TSeverityEnum> converter,
		InputInverterErrorList<TOutput, TInput, TSeverityEnum> inverter, TInput initialInput,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets) {

		Converter = converter;
		Inverter = inverter;

		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		_InputObject = initialInput;
		InputObject = initialInput;
		OnOutputObjectChanged();
	}



	public sealed override void Validate() {

		(Optional<TOutput> outputObject, ConversionErrors) = Converter(InputObject);

		ValidationErrors.Clear();

		if (outputObject.HasValue) {

			OutputObject = outputObject.Value;

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddRange(trigger.InvokeValidator());
			}

		} else {
			//OutputObject = default;
			OnOutputObjectChanged();
		}

		OnErrorsChanged();
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	private void OnOutputObjectChanged() {

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