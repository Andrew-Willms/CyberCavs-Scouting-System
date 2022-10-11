using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation.Delegates;



public delegate
	ReadOnlyList<ValidationError<TSeverity>>
	OnChangeValidator<in TOutput, TSeverity>
	(TOutput outputObject)
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity>;

public delegate
	ReadOnlyList<ValidationError<TSeverity>>
	TriggeredValidator<TOutput, in TValidationParameter, TSeverity>
	(TOutput outputObject, IInput<TOutput, TSeverity> validatee, TValidationParameter parameter)
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity>;