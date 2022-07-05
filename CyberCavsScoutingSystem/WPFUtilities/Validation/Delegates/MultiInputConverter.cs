using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



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
