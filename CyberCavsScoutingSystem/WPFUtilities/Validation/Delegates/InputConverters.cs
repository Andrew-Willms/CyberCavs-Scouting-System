using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputConverterErrorList<TOutput, in TInput, TSeverityEnum>(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TOutput?, ValidationError<TSeverityEnum>?)
	InputConverterSingleError<TOutput, in TInput, TSeverityEnum>(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TInput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputInverterErrorList<in TOutput, TInput, TSeverityEnum>(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate (TInput?, ValidationError<TSeverityEnum>?)
	InputInverterSingleError<in TOutput, TInput, TSeverityEnum>(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;