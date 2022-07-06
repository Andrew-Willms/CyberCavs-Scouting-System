using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate (TOutput?, ValidationError<TSeverityEnum>?)
	SingleInputConverterSingleError<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputConverterErrorList<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TInput?, ValidationError<TSeverityEnum>?)
	SingleInputInverterSingleError<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TInput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputInverterErrorList<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;