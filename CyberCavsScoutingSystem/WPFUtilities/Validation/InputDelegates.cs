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



public delegate (TInput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputInverter<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1>
	(TInput1 componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2>
	(TInput1 componentInput1,
		TInput2 componentInput2)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11,
		in TInput12>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11,
		TInput12 componentInput12)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11,
		in TInput12,
		in TInput13>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11,
		TInput12 componentInput12,
		TInput13 componentInput13)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11,
		in TInput12,
		in TInput13,
		in TInput14>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11,
		TInput12 componentInput12,
		TInput13 componentInput13,
		TInput14 componentInput14)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11,
		in TInput12,
		in TInput13,
		in TInput14,
		in TInput15>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11,
		TInput12 componentInput12,
		TInput13 componentInput13,
		TInput14 componentInput14,
		TInput15 componentInput15)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TOutput, TSeverityEnum,
		in TInput1,
		in TInput2,
		in TInput3,
		in TInput4,
		in TInput5,
		in TInput6,
		in TInput7,
		in TInput8,
		in TInput9,
		in TInput10,
		in TInput11,
		in TInput12,
		in TInput13,
		in TInput14,
		in TInput15,
		in TInput16>
	(TInput1 componentInput1,
		TInput2 componentInput2,
		TInput3 componentInput3,
		TInput4 componentInput4,
		TInput5 componentInput5,
		TInput6 componentInput6,
		TInput7 componentInput7,
		TInput8 componentInput8,
		TInput9 componentInput9,
		TInput10 componentInput10,
		TInput11 componentInput11,
		TInput12 componentInput12,
		TInput13 componentInput13,
		TInput14 componentInput14,
		TInput15 componentInput15,
		TInput16 componentInput16)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (
	TInput1?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	TInput12?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11,
		TInput12>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	TInput12?,
	TInput13?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11,
		TInput12,
		TInput13>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	TInput12?,
	TInput13?,
	TInput14?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11,
		TInput12,
		TInput13,
		TInput14>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	TInput12?,
	TInput13?,
	TInput14?,
	TInput15?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11,
		TInput12,
		TInput13,
		TInput14,
		TInput15>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (
	TInput1?,
	TInput2?,
	TInput3?,
	TInput4?,
	TInput5?,
	TInput6?,
	TInput7?,
	TInput8?,
	TInput9?,
	TInput10?,
	TInput11?,
	TInput12?,
	TInput13?,
	TInput14?,
	TInput15?,
	TInput16?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverter<in TOutput, TSeverityEnum,
		TInput1,
		TInput2,
		TInput3,
		TInput4,
		TInput5,
		TInput6,
		TInput7,
		TInput8,
		TInput9,
		TInput10,
		TInput11,
		TInput12,
		TInput13,
		TInput14,
		TInput15,
		TInput16>
	(TOutput componentInput1)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public record ConversionPair<TOutput, TInput, TSeverityEnum>(
	SingleInputConverter<TOutput?, TInput, TSeverityEnum> Converter,
	SingleInputInverter<TOutput, TInput?, TSeverityEnum> Inverter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;