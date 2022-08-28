using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilitiesLibrary.Extensions;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Delegates;

namespace UtilitiesLibrary.Validation.Inputs;



public interface IMultiInput<TOutput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public abstract class MultiInput<TOutput, TSeverityEnum> : Input<TOutput, TSeverityEnum>, IMultiInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private ReadOnlyList<IInput<TSeverityEnum>> InputComponents { get; }

	public override ValidationEvent OutputObjectChanged { get; } = new();

	protected abstract (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker { get; }

	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	private ReadOnlyList<ValidationError<TSeverityEnum>> ComponentErrors => InputComponents.SelectMany(x => x.Errors).ToReadOnly();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRanges(ValidationErrors, ComponentErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ComponentErrorLevel => ComponentErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Operations.Max(ConversionErrorLevel, ValidationErrorLevel, ComponentErrorLevel);

	protected bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;



	protected MultiInput(ReadOnlyList<IInput<TSeverityEnum>> inputComponents,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets) {

		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		InputComponents = inputComponents;

		foreach (IInput<TSeverityEnum> inputString in InputComponents) {
			inputString.OutputObjectChanged.Subscribe(Validate);
		}
	}



	public sealed override void Validate() {

		(Optional<TOutput> outputObject, ConversionErrors) = ConverterInvoker;

		ValidationErrors.Clear();

		OutputObject = outputObject;

		if (OutputObject.HasValue) {

			foreach (IValidationTrigger<TSeverityEnum> trigger in ValidationTriggers) {
				ValidationErrors.AddRange(trigger.InvokeValidator());
			}
		}

		OnErrorsChanged();
	}

	public override event PropertyChangedEventHandler? PropertyChanged;

	protected void OnOutputObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}




public class MultiInput<TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3>
	: MultiInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private Optional<TOutput> _OutputObject;
	public override Optional<TOutput> OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : Optional.NoValue;

		protected set {

			if (value.Equals(_OutputObject)) {
				return;
			}

			if (value.HasValue) {

				(Optional<(
					TInput1 inputComponent1,
					TInput2 inputComponent2,
					TInput3 inputComponent3)
				> _, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value.Value);

				if (errors.AreFatal()) {
					throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
				}
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private InputConverterErrorList<TOutput, 
		(TInput1,
		TInput2,
		TInput3),
		TSeverityEnum> Converter { get; }

	protected override (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker {
		get {

			if (!InputComponent1.OutputObject.HasValue ||
			    !InputComponent2.OutputObject.HasValue ||
			    !InputComponent3.OutputObject.HasValue) {

				return (Optional.NoValue, ReadOnlyList<ValidationError<TSeverityEnum>>.Empty);
			}

			return Converter((
				InputComponent1.OutputObject.Value,
				InputComponent2.OutputObject.Value,
				InputComponent3.OutputObject.Value));
		}
	}


	private InputInverterErrorList<TOutput,
		(TInput1,
		TInput2,
		TInput3),
		TSeverityEnum> Inverter { get; }

	public IInput<TInput1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TInput2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TInput3, TSeverityEnum> InputComponent3 { get; }

	public MultiInput(ConversionPair<TOutput,
			(TInput1,
			TInput2,
			TInput3),
			TSeverityEnum> conversionPair,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)
		: this(conversionPair.Converter, conversionPair.Inverter,
			inputComponent1,
			inputComponent2,
			inputComponent3,
			validationSets) { }

	public MultiInput(InputConverterErrorList<TOutput,
			(TInput1,
			TInput2,
			TInput3),
			TSeverityEnum> converter,
		InputInverterErrorList<TOutput,
			(TInput1,
			TInput2,
			TInput3),
			TSeverityEnum> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(
			inputComponent1,
			inputComponent2,
			inputComponent3),
			validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
	}

	public override void SetOutputValue(TOutput value) {

		(Optional<(
			TInput1 inputComponent1,
			TInput2 inputComponent2,
			TInput3 inputComponent3
		)> inversionResult, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

		if (errors.AreFatal()) {
			throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
		}

		// TODO: figure out if this is a good idea
		//if (inversionResult.Value.inputComponent1 is not null &&
		//    !inversionResult.Value.inputComponent1.Equals(InputComponent1.OutputObject)) {

		//	InputComponent1.OutputObject = inversionResult.Value.inputComponent1;
		//}

		InputComponent1.SetOutputValue(inversionResult.Value.inputComponent1);
		InputComponent2.SetOutputValue(inversionResult.Value.inputComponent2);
		InputComponent3.SetOutputValue(inversionResult.Value.inputComponent3);

		OutputObject = Optional<TOutput>.FromValue(value);
	}

}



public class MultiInput<TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4>
	: MultiInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private Optional<TOutput> _OutputObject;
	public override Optional<TOutput> OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : Optional.NoValue;

		protected set {

			if (value.Equals(_OutputObject)) {
				return;
			}

			if (value.HasValue) {

				(Optional<(
					TInput1 inputComponent1,
					TInput2 inputComponent2,
					TInput3 inputComponent3,
					TInput4 inputComponent4)
				> _, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value.Value);

				if (errors.AreFatal()) {
					throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
				}
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private InputConverterErrorList<TOutput, 
		(TInput1,
		TInput2,
		TInput3,
		TInput4),
		TSeverityEnum> Converter { get; }

	protected override (Optional<TOutput>, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter((
			InputComponent1.OutputObject.Value,
			InputComponent2.OutputObject.Value,
			InputComponent3.OutputObject.Value,
			InputComponent4.OutputObject.Value));

	private InputInverterErrorList<TOutput,
		(TInput1,
		TInput2,
		TInput3,
		TInput4),
		TSeverityEnum> Inverter { get; }

	public IInput<TInput1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TInput2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TInput3, TSeverityEnum> InputComponent3 { get; }
	public IInput<TInput4, TSeverityEnum> InputComponent4 { get; }

	public MultiInput(ConversionPair<TOutput,
			(TInput1,
			TInput2,
			TInput3,
			TInput4),
			TSeverityEnum> conversionPair,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		IInput<TInput4, TSeverityEnum> inputComponent4,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)
		: this(conversionPair.Converter, conversionPair.Inverter,
			inputComponent1,
			inputComponent2,
			inputComponent3,
			inputComponent4,
			validationSets) { }

	public MultiInput(InputConverterErrorList<TOutput,
			(TInput1,
			TInput2,
			TInput3,
			TInput4),
			TSeverityEnum> converter,
		InputInverterErrorList<TOutput,
			(TInput1,
			TInput2,
			TInput3,
			TInput4),
			TSeverityEnum> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		IInput<TInput4, TSeverityEnum> inputComponent4,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(
			inputComponent1,
			inputComponent2,
			inputComponent3,
			inputComponent4),
			validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;

		Validate();
	}

	public override void SetOutputValue(TOutput value) {

		(Optional<(
			TInput1 inputComponent1,
			TInput2 inputComponent2,
			TInput3 inputComponent3,
			TInput4   inputComponent4)
		> inversionResult, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

		if (errors.AreFatal()) {
			throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
		}

		// TODO: figure out if this is a good idea
		//if (inversionResult.Value.inputComponent1 is not null &&
		//    !inversionResult.Value.inputComponent1.Equals(InputComponent1.OutputObject)) {

		//	InputComponent1.OutputObject = inversionResult.Value.inputComponent1;
		//}

		InputComponent1.SetOutputValue(inversionResult.Value.inputComponent1);
		InputComponent2.SetOutputValue(inversionResult.Value.inputComponent2);
		InputComponent3.SetOutputValue(inversionResult.Value.inputComponent3);
		InputComponent4.SetOutputValue(inversionResult.Value.inputComponent4);

		OutputObject = Optional<TOutput>.FromValue(value);
	}

}