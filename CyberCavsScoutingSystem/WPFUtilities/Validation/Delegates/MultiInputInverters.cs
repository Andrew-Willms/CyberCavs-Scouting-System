using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate (
	TInput1?,
	ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputInverters<in TOutput, TSeverityEnum,
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