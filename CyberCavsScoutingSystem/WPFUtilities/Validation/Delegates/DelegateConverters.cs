using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation.Delegates; 



internal static class DelegateConverters {
	
	public static InputConverterErrorList<TOutput?, TInput, TSeverityEnum>
		SingleToErrorListConvert<TOutput, TInput, TSeverityEnum>
		(InputConverterSingleError<TOutput?, TInput, TSeverityEnum> converter)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return inputObject => {
			(TOutput? outputObject, ValidationError<TSeverityEnum>? error) = converter.Invoke(inputObject);

			return error is null
				? (outputObject, ReadOnlyList<ValidationError<TSeverityEnum>>.Empty)
				: (default, new(error));
		};
	}

	public static InputInverterErrorList<TOutput, TInput?, TSeverityEnum>
		SingleToErrorListInvert<TOutput, TInput, TSeverityEnum>
		(InputInverterSingleError<TOutput, TInput?, TSeverityEnum> inverter)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return outputObject => {
			(TInput? invertedObject, ValidationError<TSeverityEnum>? error) = inverter.Invoke(outputObject);

			return error is null
				? (invertedObject, ReadOnlyList<ValidationError<TSeverityEnum>>.Empty)
				: (default, new(error));
		};
	}

	public static InputValidatorErrorsList<TOutput, TSeverityEnum>
		SingleToErrorListValidator<TOutput, TSeverityEnum>
		(InputValidatorSingleError<TOutput, TSeverityEnum> validator)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return outputObject => {
			ValidationError<TSeverityEnum>? error = validator.Invoke(outputObject);
			return error is null ? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty : new(error);
		};
	}

	public static InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum>
		SingleToErrorListValidator<TOutput, TValidationParameter, TSeverityEnum>
		(InputValidatorSingleError<TOutput, TValidationParameter, TSeverityEnum> validator)
		where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

		return (outputObject, validationParameter) => {
			ValidationError<TSeverityEnum>? error = validator.Invoke(outputObject, validationParameter);
			return error is null ? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty : new(error);
		};
	}
}