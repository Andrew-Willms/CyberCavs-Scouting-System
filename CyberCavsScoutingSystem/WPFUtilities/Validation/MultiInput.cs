using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using WPFUtilities.Extensions;
using System;

namespace WPFUtilities.Validation;



public interface IMultiInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public ReadOnlyDictionary<string, ISingleInput<TSeverityEnum>> StringInputs { get; }
}

public interface IMultiInput<TOutput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public abstract class MultiInput<TOutput, TSeverityEnum> : Input<TOutput, TSeverityEnum>, IMultiInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private ReadOnlyList<IInput<TSeverityEnum>> InputComponents { get; }

	public override ValidationEvent OutputObjectChanged { get; } = new();

	protected abstract (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker { get; }

	private ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationTriggers { get; }

	private ReadOnlyList<ValidationError<TSeverityEnum>> ConversionErrors { get; set; } = new();
	protected override List<ValidationError<TSeverityEnum>> ValidationErrors { get; } = new();
	private ReadOnlyList<ValidationError<TSeverityEnum>> ComponentErrors => InputComponents.SelectMany(x => x.Errors).ToReadOnly();
	public override ReadOnlyList<ValidationError<TSeverityEnum>> Errors => ConversionErrors.CopyAndAddRanges(ValidationErrors, ComponentErrors);

	private TSeverityEnum ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	private TSeverityEnum ComponentErrorLevel => ComponentErrors.Select(x => x.Severity).Max() ?? TSeverityEnum.NoError;
	public override TSeverityEnum ErrorLevel => Math.Max(ConversionErrorLevel, ValidationErrorLevel, ComponentErrorLevel);

	protected bool IsConvertible => ConversionErrorLevel.IsFatal == false;
	public override bool IsValid => ErrorLevel.IsFatal == false;



	protected MultiInput(ReadOnlyList<IInput<TSeverityEnum>> inputComponents,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets) {

		ValidationTriggers = ValidationSetsToTriggers(validationSets);

		InputComponents = inputComponents;

		foreach (IInput<TSeverityEnum> inputString in InputComponents) {
			inputString.OutputObjectChanged.Subscribe(Validate);
		}

		Validate();
	}


	public sealed override void Validate() {

		(OutputObject, ConversionErrors) = ConverterInvoker;

		ValidationErrors.Clear();

		if (OutputObject is not null) {

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
		TInput1>
	: MultiInput<TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _OutputObject;

	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : default;

		set {
			
			if (value is null) {
				throw new InvalidOperationException($"You cannot set {nameof(OutputObject)} to a null value.");
			}

			(TInput1? input1Result, ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

			if (errors.Any() == false) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			if (input1Result is not null /*&& result == InputObject*/) {
				InputComponent1.OutputObject = input1Result;
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private MultiInputConverter<TOutput?, TSeverityEnum,
		TInput1> Converter { get; }

	protected override (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!);

	private MultiInputInverter<TOutput, TSeverityEnum,
		TInput1> Inverter { get; }

	private IInput<TInput1, TSeverityEnum> InputComponent1 { get; }

	public MultiInput(MultiInputConverter<TOutput?, TSeverityEnum, 
			TInput1> converter,
		MultiInputInverter<TOutput, TSeverityEnum,
			TInput1> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
	}

}

public class MultiInput<TOutput, TSeverityEnum, 
		TInput1,
		TInput2>
	: MultiInput<TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _OutputObject;

	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : default;

		set {
			
			if (value is null) {
				throw new InvalidOperationException($"You cannot set {nameof(OutputObject)} to a null value.");
			}

			(TInput1? input1Result,
				TInput2? input2Result,
				ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

			if (errors.Any() == false) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			if (input1Result is not null /*&& result == InputObject*/) {
				InputComponent1.OutputObject = input1Result;
			}

			if (input2Result is not null /*&& result == InputObject*/) {
				InputComponent2.OutputObject = input2Result;
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private MultiInputConverter<TOutput?, TSeverityEnum,
		TInput1,
		TInput2> Converter { get; }

	protected override (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!);

	private MultiInputInverter<TOutput, TSeverityEnum,
		TInput1,
		TInput2> Inverter { get; }

	private IInput<TInput1, TSeverityEnum> InputComponent1 { get; }
	private IInput<TInput2, TSeverityEnum> InputComponent2 { get; }

	public MultiInput(MultiInputConverter<TOutput?, TSeverityEnum, 
			TInput1,
			TInput2> converter,
		MultiInputInverter<TOutput, TSeverityEnum,
			TInput1,
			TInput2> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
	}

}

public class MultiInput<TOutput, TSeverityEnum, 
		TInput1,
		TInput2,
		TInput3>
	: MultiInput<TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _OutputObject;

	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : default;

		set {
			
			if (value is null) {
				throw new InvalidOperationException($"You cannot set {nameof(OutputObject)} to a null value.");
			}

			(TInput1? input1Result,
				TInput2? input2Result,
				TInput3? input3Result,
				ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

			if (errors.Any() == false) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			if (input1Result is not null /*&& result == InputObject*/) {
				InputComponent1.OutputObject = input1Result;
			}

			if (input2Result is not null /*&& result == InputObject*/) {
				InputComponent2.OutputObject = input2Result;
			}

			if (input3Result is not null /*&& result == InputObject*/) {
				InputComponent3.OutputObject = input3Result;
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private MultiInputConverter<TOutput?, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3> Converter { get; }

	protected override (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!);

	private MultiInputInverter<TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3> Inverter { get; }

	private IInput<TInput1, TSeverityEnum> InputComponent1 { get; }
	private IInput<TInput2, TSeverityEnum> InputComponent2 { get; }
	private IInput<TInput3, TSeverityEnum> InputComponent3 { get; }

	public MultiInput(MultiInputConverter<TOutput?, TSeverityEnum, 
			TInput1,
			TInput2,
			TInput3> converter,
		MultiInputInverter<TOutput, TSeverityEnum,
			TInput1,
			TInput2,
			TInput3> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
	}

}

public class MultiInput<TOutput, TSeverityEnum, 
		TInput1,
		TInput2,
		TInput3,
		TInput4>
	: MultiInput<TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _OutputObject;

	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _OutputObject : default;

		set {
			
			if (value is null) {
				throw new InvalidOperationException($"You cannot set {nameof(OutputObject)} to a null value.");
			}

			(TInput1? input1Result,
				TInput2? input2Result,
				TInput3? input3Result,
				TInput4? input4Result,
				ReadOnlyList<ValidationError<TSeverityEnum>> errors) = Inverter(value);

			if (errors.Any() == false) {
				throw new InvalidOperationException($"You are setting {nameof(OutputObject)} to an invalid value.");
			}

			if (input1Result is not null /*&& result == InputObject*/) {
				InputComponent1.OutputObject = input1Result;
			}

			if (input2Result is not null /*&& result == InputObject*/) {
				InputComponent2.OutputObject = input2Result;
			}

			if (input3Result is not null /*&& result == InputObject*/) {
				InputComponent3.OutputObject = input3Result;
			}

			if (input4Result is not null /*&& result == InputObject*/) {
				InputComponent4.OutputObject = input4Result;
			}

			_OutputObject = value;
			OnOutputObjectChanged();
		}
	}

	private MultiInputConverter<TOutput?, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4> Converter { get; }

	protected override (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!,
			InputComponent4.OutputObject!);

	private MultiInputInverter<TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4> Inverter { get; }

	private IInput<TInput1, TSeverityEnum> InputComponent1 { get; }
	private IInput<TInput2, TSeverityEnum> InputComponent2 { get; }
	private IInput<TInput3, TSeverityEnum> InputComponent3 { get; }
	private IInput<TInput4, TSeverityEnum> InputComponent4 { get; }

	public MultiInput(MultiInputConverter<TOutput?, TSeverityEnum, 
			TInput1,
			TInput2,
			TInput3,
			TInput4> converter,
		MultiInputInverter<TOutput, TSeverityEnum,
			TInput1,
			TInput2,
			TInput3,
			TInput4> inverter,
		IInput<TInput1, TSeverityEnum> inputComponent1,
		IInput<TInput2, TSeverityEnum> inputComponent2,
		IInput<TInput3, TSeverityEnum> inputComponent3,
		IInput<TInput4, TSeverityEnum> inputComponent4,
		params IValidationSet<TOutput, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;
		Inverter = inverter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;
	}

}