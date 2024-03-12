using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Validation.Inputs;



public interface IMultiInput<TOutput, TSeverity> : IInput<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {
}



public abstract class MultiInput<TOutput, TSeverity> : Input<TOutput, TSeverity>, IMultiInput<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private ReadOnlyList<IInput<TSeverity>> InputComponents { get; }

	protected abstract (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverity>>) ConverterInvoker { get; }

	public ReadOnlyList<ValidationError<TSeverity>> ComponentErrors => InputComponents.SelectMany(x => x.Errors).ToReadOnly();
	public override ReadOnlyList<ValidationError<TSeverity>> Errors => ConversionErrors.CopyAndAddRanges(ValidationErrors, ComponentErrors);

	public TSeverity ComponentErrorLevel => ComponentErrors.Select(x => x.Severity).Max() ?? TSeverity.NoError;



	protected MultiInput(ReadOnlyList<IInput<TSeverity>> inputComponents,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets)
		: base(validationSets) {

		InputComponents = inputComponents;
		InputComponents.Foreach(x => x.OutputObjectChanged.Subscribe(Validate));
	}



	public sealed override void Validate() {

		(Optional<TOutput> outputObject, ConversionErrors) = ConverterInvoker;

		OutputObject = outputObject;
		OnErrorsChanged();
	}

	public new event PropertyChangedEventHandler? PropertyChanged;

	protected void OnOutputObjectChanged() {
		PropertyChanged?.Invoke(this, new(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {

		PropertyChanged?.Invoke(this, new(nameof(Errors)));
		PropertyChanged?.Invoke(this, new(nameof(ValidationErrors)));
		PropertyChanged?.Invoke(this, new(nameof(ComponentErrors)));
		PropertyChanged?.Invoke(this, new(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new(nameof(ValidationErrorLevel)));
		PropertyChanged?.Invoke(this, new(nameof(ComponentErrorLevel)));
		PropertyChanged?.Invoke(this, new(nameof(IsValid)));
	}

}



public class MultiInput<TOutput, TSeverity,
		TInput1, TInput2, TInput3>
	: MultiInput<TOutput, TSeverity>
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
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private InputConverter<TOutput,
		(TInput1, TInput2, TInput3),
		TSeverity> Converter { get; }

	protected override (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverity>>) ConverterInvoker {

		get {

			if (InputComponent1.OutputObject.HasValue &&
				InputComponent2.OutputObject.HasValue &&
				InputComponent3.OutputObject.HasValue) {

				return Converter((
					InputComponent1.OutputObject.Value,
					InputComponent2.OutputObject.Value,
					InputComponent3.OutputObject.Value));
			}

			return (Optional.Optional.NoValue, ReadOnlyList.Empty);

		}
	}

	private InputInverter<TOutput,
		(TInput1, TInput2, TInput3),
		TSeverity> Inverter { get; }

	private IInput<TInput1, TSeverity> InputComponent1 { get; }
	private IInput<TInput2, TSeverity> InputComponent2 { get; }
	private IInput<TInput3, TSeverity> InputComponent3 { get; }



	protected internal MultiInput(
		InputConverter<TOutput,
			(TInput1, TInput2, TInput3),
			TSeverity> converter,
		InputInverter<TOutput,
			(TInput1, TInput2, TInput3),
			TSeverity> inverter,
		IInput<TInput1, TSeverity> inputComponent1,
		IInput<TInput2, TSeverity> inputComponent2,
		IInput<TInput3, TSeverity> inputComponent3,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets)

		: base(
			new List<IInput<TSeverity>> { inputComponent1, inputComponent2, inputComponent3 }.ToReadOnly(),
			validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;

		Validate();
	}

	protected override bool HasValueAndIsNotInvertible(Optional<TOutput> testValue) {

		if (!testValue.HasValue) {
			return false;
		}

		(Optional<(TInput1, TInput2, TInput3)> inversionResult,
			ReadOnlyList<ValidationError<TSeverity>> errors) = Inverter(testValue.Value);

		return errors.AreFatal() || !inversionResult.HasValue;
	}

}



public class MultiInput<TOutput, TSeverity,
		TInput1, TInput2, TInput3, TInput4>
	: MultiInput<TOutput, TSeverity>
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
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private InputConverter<TOutput,
		(TInput1, TInput2, TInput3, TInput4),
		TSeverity> Converter { get; }

	protected override (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverity>>) ConverterInvoker {

		get {

			if (InputComponent1.OutputObject.HasValue &&
				InputComponent2.OutputObject.HasValue &&
				InputComponent3.OutputObject.HasValue &&
				InputComponent4.OutputObject.HasValue) {

				return Converter((
					InputComponent1.OutputObject.Value,
					InputComponent2.OutputObject.Value,
					InputComponent3.OutputObject.Value,
					InputComponent4.OutputObject.Value));
			}

			return (Optional.Optional.NoValue, ReadOnlyList.Empty);

		}
	}


	private InputInverter<TOutput,
		(TInput1, TInput2, TInput3, TInput4),
		TSeverity> Inverter { get; }

	private IInput<TInput1, TSeverity> InputComponent1 { get; }
	private IInput<TInput2, TSeverity> InputComponent2 { get; }
	private IInput<TInput3, TSeverity> InputComponent3 { get; }
	private IInput<TInput4, TSeverity> InputComponent4 { get; }



	protected internal MultiInput(
		InputConverter<TOutput,
			(TInput1, TInput2, TInput3, TInput4),
			TSeverity> converter,
		InputInverter<TOutput,
			(TInput1, TInput2, TInput3, TInput4),
			TSeverity> inverter,
		IInput<TInput1, TSeverity> inputComponent1,
		IInput<TInput2, TSeverity> inputComponent2,
		IInput<TInput3, TSeverity> inputComponent3,
		IInput<TInput4, TSeverity> inputComponent4,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets)

		: base(
			new List<IInput<TSeverity>> { inputComponent1, inputComponent2, inputComponent3, inputComponent4 }.ToReadOnly(),
			validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;

		Validate();
	}

	protected override bool HasValueAndIsNotInvertible(Optional<TOutput> testValue) {

		if (!testValue.HasValue) {
			return false;
		}

		(Optional<(TInput1, TInput2, TInput3, TInput4)> inversionResult,
			ReadOnlyList<ValidationError<TSeverity>> errors) = Inverter(testValue.Value);

		return errors.AreFatal() || !inversionResult.HasValue;
	}

}