using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Delegates; 



internal static class DelegateConverters {
	
	public static InputConverterErrorList<TOutput, TInput, TSeverityEnum>
		SingleToErrorListConvert<TOutput, TInput, TSeverityEnum>
		(InputConverterSingleError<TOutput, TInput, TSeverityEnum> converter)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return inputObject => {

			(Optional<TOutput> outputObject, Optional<ValidationError<TSeverityEnum>> error) = converter.Invoke(inputObject);

			ReadOnlyList<ValidationError<TSeverityEnum>> errors = error.HasValue
				? new(error.Value)
				: ReadOnlyList<ValidationError<TSeverityEnum>>.Empty;

			return outputObject.HasValue
				? (outputObject, errors)
				: (default, errors);
		};
	}

	public static InputInverterErrorList<TOutput, TInput, TSeverityEnum>
		SingleToErrorListInvert<TOutput, TInput, TSeverityEnum>
		(InputInverterSingleError<TOutput, TInput, TSeverityEnum> inverter)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return outputObject => {

			(Optional<TInput> invertedObject, Optional<ValidationError<TSeverityEnum>> error) = inverter.Invoke(outputObject);

			ReadOnlyList<ValidationError<TSeverityEnum>> errors = error.HasValue
				? new(error.Value)
				: ReadOnlyList<ValidationError<TSeverityEnum>>.Empty;

			return invertedObject.HasValue
				? (invertedObject, errors)
				: (default, errors);
		};
	}

	public static InputValidatorErrorsList<TOutput, TSeverityEnum>
		SingleToErrorListValidator<TOutput, TSeverityEnum>
		(InputValidatorSingleError<TOutput, TSeverityEnum> validator)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return outputObject => {

			Optional<ValidationError<TSeverityEnum>> error = validator.Invoke(outputObject);

			return error.HasValue ? new(error.Value) : ReadOnlyList<ValidationError<TSeverityEnum>>.Empty;
		};
	}

	public static InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum>
		SingleToErrorListValidator<TOutput, TValidationParameter, TSeverityEnum>
		(InputValidatorSingleError<TOutput, TValidationParameter, TSeverityEnum> validator)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return (outputObject, validationParameter) => {

			Optional<ValidationError<TSeverityEnum>> error = validator.Invoke(outputObject, validationParameter);

			return error.HasValue ? new(error.Value) : ReadOnlyList<ValidationError<TSeverityEnum>>.Empty;
		};
	}
}