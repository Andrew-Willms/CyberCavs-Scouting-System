using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation.Delegates;



public delegate
	ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<in TOutput, TSeverityEnum>
	(TOutput outputObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate
	ReadOnlyList<ValidationError<TSeverityEnum>>
	InputValidator<TOutput, in TValidationParameter, TSeverityEnum>
	(TOutput outputObject, IInput<TOutput, TSeverityEnum> validatee, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;