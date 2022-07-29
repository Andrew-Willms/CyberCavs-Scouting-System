using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate
	(Optional<TOutput>, Optional<ValidationError<TSeverityEnum>>)
	InputConverterSingleError<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate
	(Optional<TOutput>, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputConverterErrorList<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate
	(Optional<TInput>, Optional<ValidationError<TSeverityEnum>>)
	InputInverterSingleError<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate
	(Optional<TInput>, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputInverterErrorList<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;