using System;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation;



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
		: this(DelegateConverters.SingleToErrorListValidator(validator), validationEvents) { }

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
		: this(DelegateConverters.SingleToErrorListValidator(validator), validationParameterGetter, validationEvents) { }

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