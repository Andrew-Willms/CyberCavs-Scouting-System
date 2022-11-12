using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

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

	private InputConverter<TOutput, TInput, TSeverity> Converter { get; }
	private InputInverter<TOutput, TInput, TSeverity> Inverter { get; }

	private ReadOnlyList<ValidationError<TSeverity>> ConversionErrors { get; set; } = new();
	public override ReadOnlyList<ValidationError<TSeverity>> Errors => ConversionErrors.AppendRange(ValidationErrors).ToReadOnly();

	private TSeverity ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverity.NoError;
	private TSeverity ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverity.NoError;
	public override TSeverity ErrorLevel => Math.Operations.Max(ConversionErrorLevel, ValidationErrorLevel);

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;

	public override ValidationEvent OutputObjectChanged { get; } = new();



	protected internal SingleInput(
		InputConverter<TOutput, TInput, TSeverity> converter,
		InputInverter<TOutput, TInput, TSeverity> inverter,
		TInput initialInput,
		IEnumerable<OnChangeValidator<TOutput, TSeverity>> onChangeValidators,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets)
		: base(onChangeValidators, validationSets) {

		Converter = converter;
		Inverter = inverter;
		InputObject = initialInput;
	}



	public sealed override void Validate() {

		(Optional<TOutput> outputObject, ConversionErrors) = Converter(InputObject);

		foreach (OnChangeValidator<TOutput, TSeverity> validator in OnChangedValidation.Keys) {

			OnChangedValidation[validator] = outputObject.HasValue
				? validator.Invoke(outputObject.Value)
				: ReadOnlyList<ValidationError<TSeverity>>.Empty;
		}

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
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}