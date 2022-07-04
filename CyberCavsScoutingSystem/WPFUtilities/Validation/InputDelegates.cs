using System;

namespace WPFUtilities.Validation;



public struct Optional<T> {

	private T? _Value = default;
	public T Value {

		get {
			if (!HasValue) {
				throw new InvalidOperationException("You cannot get the value of a null nullable.");
			}

			return _Value;
		}

		set => _Value = value;
	}

	public bool HasValue { get; }

	public Optional() {

		HasValue = false;
	}

	public Optional(T value) {

		HasValue = true;
		Value = value;
	}

}



/*
When value types are used in StringInputValidator<TheValueType, TSeverityEnum> the method cannot return a nullable
version of that value type because C# handles nullable reference types and nullable value types differently
*/

public delegate ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<in TOutput, TSeverityEnum>
	(TOutput targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<in TOutput, in TValidationParameter, TSeverityEnum>
	(TOutput targetObject, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputConverter<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;




public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TComponent1>
	(TComponent1 componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2>
	(TComponent1 componentInput1,
		TComponent2 componentInput2)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11,
		in TComponent12>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11,
		TComponent12 componentInput12)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11,
		in TComponent12,
		in TComponent13>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11,
		TComponent12 componentInput12,
		TComponent13 componentInput13)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11,
		in TComponent12,
		in TComponent13,
		in TComponent14>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11,
		TComponent12 componentInput12,
		TComponent13 componentInput13,
		TComponent14 componentInput14)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11,
		in TComponent12,
		in TComponent13,
		in TComponent14,
		in TComponent15>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11,
		TComponent12 componentInput12,
		TComponent13 componentInput13,
		TComponent14 componentInput14,
		TComponent15 componentInput15)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TTarget?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTarget, TSeverityEnum,
		in TComponent1,
		in TComponent2,
		in TComponent3,
		in TComponent4,
		in TComponent5,
		in TComponent6,
		in TComponent7,
		in TComponent8,
		in TComponent9,
		in TComponent10,
		in TComponent11,
		in TComponent12,
		in TComponent13,
		in TComponent14,
		in TComponent15,
		in TComponent16>
	(TComponent1 componentInput1,
		TComponent2 componentInput2,
		TComponent3 componentInput3,
		TComponent4 componentInput4,
		TComponent5 componentInput5,
		TComponent6 componentInput6,
		TComponent7 componentInput7,
		TComponent8 componentInput8,
		TComponent9 componentInput9,
		TComponent10 componentInput10,
		TComponent11 componentInput11,
		TComponent12 componentInput12,
		TComponent13 componentInput13,
		TComponent14 componentInput14,
		TComponent15 componentInput15,
		TComponent16 componentInput16)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;