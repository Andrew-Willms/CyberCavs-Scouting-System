using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using WPFUtilities.Extensions;

namespace WPFUtilities.Validation;



public interface IMultiInput<TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public ReadOnlyDictionary<string, ISingleInput<TSeverityEnum>> StringInputs { get; }
}

public interface IMultiInput<out TOutput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {
}



public abstract class MultiInput<TOutput, TSeverityEnum> : Input<TOutput, TSeverityEnum>, IMultiInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TOutput? _TargetObject;
	public override TOutput? OutputObject {

		// TODO: .Net 7.0 remove backing field
		get => IsConvertible ? _TargetObject : default;

		protected set {
			_TargetObject = value;
			OnTargetObjectChanged();
		}
	}

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

	private bool IsConvertible => ConversionErrorLevel.IsFatal == false;
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

	private void OnTargetObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutputObject)));
		OutputObjectChanged.Invoke();
	}

	protected override void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
	}

}



public class MultiInput<TTarget, TSeverityEnum, 
		TComponent1>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
	}

}

public class MultiInput<TTarget, TSeverityEnum, 
		TComponent1,
		TComponent2>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1,
		TComponent2> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TComponent2, TSeverityEnum> InputComponent2 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1,
			TComponent2> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		IInput<TComponent2, TSeverityEnum> inputComponent2,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
	}

}

public class MultiInput<TTarget, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TComponent2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TComponent3, TSeverityEnum> InputComponent3 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1,
			TComponent2,
			TComponent3> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		IInput<TComponent2, TSeverityEnum> inputComponent2,
		IInput<TComponent3, TSeverityEnum> inputComponent3,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
	}

}

public class MultiInput<TTarget, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!,
			InputComponent4.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TComponent2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TComponent3, TSeverityEnum> InputComponent3 { get; }
	public IInput<TComponent4, TSeverityEnum> InputComponent4 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1,
			TComponent2,
			TComponent3,
			TComponent4> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		IInput<TComponent2, TSeverityEnum> inputComponent2,
		IInput<TComponent3, TSeverityEnum> inputComponent3,
		IInput<TComponent4, TSeverityEnum> inputComponent4,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;
	}

}

public class MultiInput<TTarget, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4,
		TComponent5>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4,
		TComponent5> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!,
			InputComponent4.OutputObject!,
			InputComponent5.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TComponent2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TComponent3, TSeverityEnum> InputComponent3 { get; }
	public IInput<TComponent4, TSeverityEnum> InputComponent4 { get; }
	public IInput<TComponent5, TSeverityEnum> InputComponent5 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1,
			TComponent2,
			TComponent3,
			TComponent4,
			TComponent5> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		IInput<TComponent2, TSeverityEnum> inputComponent2,
		IInput<TComponent3, TSeverityEnum> inputComponent3,
		IInput<TComponent4, TSeverityEnum> inputComponent4,
		IInput<TComponent5, TSeverityEnum> inputComponent5,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;
		InputComponent5 = inputComponent5;
	}

}

public class MultiInput<TTarget, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4,
		TComponent5,
		TComponent6>
	: MultiInput<TTarget, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private MultiInputConverter<TTarget?, TSeverityEnum,
		TComponent1,
		TComponent2,
		TComponent3,
		TComponent4,
		TComponent5,
		TComponent6> Converter { get; }

	protected override (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>) ConverterInvoker
		=> Converter(InputComponent1.OutputObject!,
			InputComponent2.OutputObject!,
			InputComponent3.OutputObject!,
			InputComponent4.OutputObject!,
			InputComponent5.OutputObject!,
			InputComponent6.OutputObject!);

	public IInput<TComponent1, TSeverityEnum> InputComponent1 { get; }
	public IInput<TComponent2, TSeverityEnum> InputComponent2 { get; }
	public IInput<TComponent3, TSeverityEnum> InputComponent3 { get; }
	public IInput<TComponent4, TSeverityEnum> InputComponent4 { get; }
	public IInput<TComponent5, TSeverityEnum> InputComponent5 { get; }
	public IInput<TComponent6, TSeverityEnum> InputComponent6 { get; }

	public MultiInput(MultiInputConverter<TTarget?, TSeverityEnum,
			TComponent1,
			TComponent2,
			TComponent3,
			TComponent4,
			TComponent5,
			TComponent6> converter,
		IInput<TComponent1, TSeverityEnum> inputComponent1,
		IInput<TComponent2, TSeverityEnum> inputComponent2,
		IInput<TComponent3, TSeverityEnum> inputComponent3,
		IInput<TComponent4, TSeverityEnum> inputComponent4,
		IInput<TComponent5, TSeverityEnum> inputComponent5,
		IInput<TComponent6, TSeverityEnum> inputComponent6,
		params IValidationSet<TTarget, TSeverityEnum>[] validationSets)

		: base(new(inputComponent1), validationSets) {

		Converter = converter;

		InputComponent1 = inputComponent1;
		InputComponent2 = inputComponent2;
		InputComponent3 = inputComponent3;
		InputComponent4 = inputComponent4;
		InputComponent5 = inputComponent5;
		InputComponent6 = inputComponent6;
	}

}