using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate (TOutput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputConverter<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TInput?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	SingleInputInverter<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;