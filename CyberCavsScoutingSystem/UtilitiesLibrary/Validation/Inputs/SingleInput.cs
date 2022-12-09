using System;
using System.Collections.Generic;
using System.ComponentModel;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Validation.Inputs;



public interface ISingleInput<TSeverity> : IInput<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> { }

public interface ISingleInput<TInput, TSeverity> : IInput<TSeverity>, ISingleInput<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public TInput InputObject { get; set; }
}

public interface ISingleInput<TOutput, TInput, TSeverity> : IInput<TOutput, TSeverity>, ISingleInput<TInput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {
}



public class SingleInput<TOutput, TInput, TSeverity> : Input<TOutput, TSeverity>, ISingleInput<TOutput, TInput, TSeverity>
	where TInput : IEquatable<TInput>
	where TOutput : IEquatable<TOutput>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private Optional<TOutput> _OutputObject = Optional.Optional.NoValue;
	public override Optional<TOutput> OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => _OutputObject;

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

	private InputConverter<TOutput, TInput, TSeverity> Converter { get; }
	private InputInverter<TOutput, TInput, TSeverity> Inverter { get; }

	public override ReadOnlyList<ValidationError<TSeverity>> Errors => ConversionErrors.AppendRange(ValidationErrors).ToReadOnly();



	protected internal SingleInput(
		InputConverter<TOutput, TInput, TSeverity> converter,
		InputInverter<TOutput, TInput, TSeverity> inverter,
		TInput initialInput,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets)
		: base(validationSets) {

		Converter = converter;
		Inverter = inverter;
		InputObject = initialInput;
	}



	public sealed override void Validate() {

		(Optional<TOutput> outputObject, ConversionErrors) = Converter(InputObject);

		OutputObject = outputObject;
		OnErrorsChanged();
	}

	protected override bool HasValueAndIsNotInvertible(Optional<TOutput> testValue) {

		if (!testValue.HasValue) {
			return false;
		}

		(Optional<TInput> inversionResult, ReadOnlyList<ValidationError<TSeverity>> errors) = Inverter(testValue.Value);

		return errors.AreFatal() || !inversionResult.HasValue;
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private void OnInputChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputObject)));
	}

	private void OnOutputObjectChanged() {
		PropertyChanged?.Invoke(this, new(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {

		PropertyChanged?.Invoke(this, new(nameof(Errors)));
		PropertyChanged?.Invoke(this, new(nameof(ValidationErrors)));
		PropertyChanged?.Invoke(this, new(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new(nameof(ValidationErrorLevel)));
		PropertyChanged?.Invoke(this, new(nameof(IsValid)));
	}

}