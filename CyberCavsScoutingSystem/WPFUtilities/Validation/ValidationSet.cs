using System;

namespace WPFUtilities.Validation; 



public interface IValidationSet<in TTargetType, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public IValidationTrigger ToValidationTrigger(ValidationEvent validationEvent, Func<TTargetType> targetObjectGetter,
		Action<ValidationError<TSeverityEnum>> postValidationAction);
}

public class ValidationSet<TTargetType, TSeverityEnum> : IValidationSet<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private StringInputValidator<TTargetType, TSeverityEnum> Validator { get; }

	private ValidationEvent? ValidationEvent { get; }

	public ValidationSet(StringInputValidator<TTargetType, TSeverityEnum> validator, ValidationEvent? validationEvent = null) {

		Validator = validator;
		ValidationEvent = validationEvent;
	}

	public IValidationTrigger ToValidationTrigger(ValidationEvent validationEvent, Func<TTargetType> targetObjectGetter,
		Action<ValidationError<TSeverityEnum>> postValidationAction) {

		return new ValidationTrigger<TTargetType, TSeverityEnum>(Validator, ValidationEvent ?? validationEvent,
			targetObjectGetter, postValidationAction);
	}
}

public class ValidationSet<TTargetType, TValidationParameter, TSeverityEnum> : IValidationSet<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private StringInputValidator<TTargetType, TValidationParameter, TSeverityEnum> Validator { get; }

	private ValidationEvent? ValidationEvent { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	public ValidationSet(StringInputValidator<TTargetType, TValidationParameter, TSeverityEnum> validator,
		Func<TValidationParameter> validationParameterGetter, ValidationEvent? validationEvent = null) {

		Validator = validator;
		ValidationEvent = validationEvent;
		ValidationParameterGetter = validationParameterGetter;
	}

	public IValidationTrigger ToValidationTrigger(ValidationEvent validationEvent, Func<TTargetType> targetObjectGetter,
		Action<ValidationError<TSeverityEnum>> postValidationAction) {

		return new ValidationTrigger<TTargetType, TValidationParameter, TSeverityEnum>(Validator, ValidationEvent ?? validationEvent,
			targetObjectGetter, ValidationParameterGetter, postValidationAction);
	}
}