using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Delegates;



public delegate
	ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<in TOutput, TSeverityEnum>
	(TOutput targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate
	ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<in TOutput, in TValidationParameter, TSeverityEnum>
	(TOutput targetObject, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;