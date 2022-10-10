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

	private Optional<TOutput> _OutputObject = Optional.NoValue;
	public override Optional<TOutput> OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : Optional.NoValue;

		protected set {

			if (value.Equals(_OutputObject)) {
				return;
			}

			if (HasValueAndIsNotInvertible(value)) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to a value that cannot be inverted.");
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private TInput _InputObject = default!;
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
	public override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
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

		InputObject = initialInput;
	}



	public sealed override void Validate() {

		(OutputObject, ConversionErrors) = Converter(InputObject);

		ValidationErrors.Clear();

		if (OutputObject.HasValue) {

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddRange(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}

	protected override bool HasValueAndIsNotInvertible(Optional<TOutput> testValue) {

		if (!testValue.HasValue) {
			return false;
		}

		(Optional<TInput> inversionResult, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(testValue.Value);

		return errors.AreFatal() || !inversionResult.HasValue;
	}



	public override event PropertyChangedEventHandler? PropertyChanged;

	private void OnInputChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputObject)));
	}

	private void OnOutputObjectChanged() {

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}