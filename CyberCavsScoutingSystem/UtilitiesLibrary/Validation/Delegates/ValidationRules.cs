using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Delegates;



public delegate
	ReadOnlyList<ValidationError<TSeverity>>
	ValidationRule<in TOutput, TSeverity>
	(TOutput outputObject)
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity>;

public delegate
	ReadOnlyList<ValidationError<TSeverity>>
	ValidationRule<in TOutput, in TValidationParameter, TSeverity>
	(TOutput outputObject, TValidationParameter parameter)
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity>;