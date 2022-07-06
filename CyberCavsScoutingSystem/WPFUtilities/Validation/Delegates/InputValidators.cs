using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates;



public delegate ValidationError<TSeverityEnum>?
	InputValidatorSingleError<in TOutput, TSeverityEnum>
	(TOutput targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ValidationError<TSeverityEnum>?
	InputValidatorSingleError<in TOutput, in TValidationParameter, TSeverityEnum>
	(TOutput targetObject, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidatorErrorsList<in TOutput, TSeverityEnum>
	(TOutput targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidatorErrorsList<in TOutput, in TValidationParameter, TSeverityEnum>
	(TOutput targetObject, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;