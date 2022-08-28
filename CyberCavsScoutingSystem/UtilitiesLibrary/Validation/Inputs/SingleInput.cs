using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Delegates;

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

	private Optional<TOutput> _OutputObject;
	public override Optional<TOutput> OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : Optional.NoValue;

		protected set {

			if (value.Equals(_OutputObject)) {
				return;
			}



			(Optional<TInput> inversionResult, ReadOnlyList<ValidationError<TSeverityEnum>> inversionErrors) = Inverter(value.Value);

			if (inversionErrors.Any(x => x.Severity.IsFatal) || !inversionResult.HasValue) {
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

	private InputConverter<TOutput, TInput, TSeverityEnum> Converter { get; }
	private InputInverter<TOutput, TInput, TSeverityEnum> Inverter { get; }

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



	public SingleInput(InputConverter<TOutput, TInput, TSeverityEnum> converter,
		InputInverter<TOutput, TInput, TSeverityEnum> inverter, TInput initialInput,
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

	public override void SetOutputValue(TOutput value) {

		(Optional<TInput> _, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

		if (errors.AreFatal()) {
			throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
		}

		OutputObject = value;
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