using System;

namespace WPFUtilities.Validation; 



public interface IValidationSet<in TTargetType, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TTargetType> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction);
}



public class ValidationSet<TTargetType, TSeverityEnum> : IValidationSet<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TTargetType, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }

	public ValidationSet(InputValidator<TTargetType, TSeverityEnum> validator, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationEvents = validationEvents;
	}

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TTargetType> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TTargetType, TSeverityEnum>(Validator, ValidationEvents,
			targetObjectGetter, postValidationAction);
	}
}



public class ValidationSet<TTargetType, TValidationParameter, TSeverityEnum> : IValidationSet<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TTargetType, TValidationParameter, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	public ValidationSet(InputValidator<TTargetType, TValidationParameter, TSeverityEnum> validator,
		Func<TValidationParameter> validationParameterGetter, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationEvents = validationEvents;
		ValidationParameterGetter = validationParameterGetter;
	}

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(Func<TTargetType> targetObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TTargetType, TValidationParameter, TSeverityEnum>(Validator, ValidationEvents,
			targetObjectGetter, ValidationParameterGetter, postValidationAction);
	}
}