using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Delegates;



public delegate
	(Optional<TOutput>, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputConverter<TOutput, in TInput, TSeverityEnum>
	(TInput input)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate
	(Optional<TInput>, ReadOnlyList<ValidationError<TSeverityEnum>>)
	InputInverter<in TOutput, TInput, TSeverityEnum>
	(TOutput output)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;