using System;
using WPFUtilities.Validation.Delegates;
using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation;



public interface IValidationSet<in TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TOutput> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction);
}



public class ValidationSet<TOutput, TSeverityEnum> : IValidationSet<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidatorErrorsList<TOutput, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }



	public ValidationSet(InputValidatorSingleError<TOutput, TSeverityEnum> validator, params ValidationEvent[] validationEvents)

		: this(outputObject => {
			ValidationError<TSeverityEnum>? error = validator.Invoke(outputObject);
			return error is null ? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty : new(error);
		}, validationEvents)
	{ }

	public ValidationSet(InputValidatorErrorsList<TOutput, TSeverityEnum> validator, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationEvents = validationEvents;
	}



	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TOutput> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TOutput, TSeverityEnum>(Validator, ValidationEvents,
			targetObjectGetter, postValidationAction);
	}

}



public class ValidationSet<TOutput, TValidationParameter, TSeverityEnum> : IValidationSet<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }



	public ValidationSet(InputValidatorSingleError<TOutput, TValidationParameter, TSeverityEnum> validator,
		Func<TValidationParameter> validationParameterGetter, params ValidationEvent[] validationEvents) 

		: this((outputObject, validationParameter) => {
			ValidationError<TSeverityEnum>? error = validator.Invoke(outputObject, validationParameter);
			return error is null ? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty : new(error);
		}, validationParameterGetter, validationEvents)
	{ }

	public ValidationSet(InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum> validator,
		Func<TValidationParameter> validationParameterGetter, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationEvents = validationEvents;
		ValidationParameterGetter = validationParameterGetter;
	}



	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TOutput> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TOutput, TValidationParameter, TSeverityEnum>(Validator, ValidationEvents,
			targetObjectGetter, ValidationParameterGetter, postValidationAction);
	}

}