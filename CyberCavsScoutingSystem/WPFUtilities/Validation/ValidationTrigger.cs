using System;

namespace WPFUtilities.Validation; 



public interface IValidationTrigger {

	public void EventHandler();
}



public class ValidationTrigger<TTargetType, TSeverityEnum> : IValidationTrigger
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private StringInputValidator<TTargetType, TSeverityEnum> Validator { get; }

	private Func<TTargetType> TargetObjectGetter { get; }

	private Action<ValidationError<TSeverityEnum>> PostValidationAction { get; }



	public ValidationTrigger(StringInputValidator<TTargetType, TSeverityEnum> validator, ValidationEvent validationEvent,
		Func<TTargetType> targetObjectGetter, Action<ValidationError<TSeverityEnum>> postValidationAction) {

		Validator = validator;
		TargetObjectGetter = targetObjectGetter;
		PostValidationAction = postValidationAction;

		validationEvent.Subscribe(EventHandler);
	}



	public void EventHandler() {

		ValidationError<TSeverityEnum>? validationError = Validator.Invoke(TargetObjectGetter.Invoke());

		if (validationError is not null) {
			PostValidationAction.Invoke(validationError);
		}
	}

}



public class ValidationTrigger<TTargetType, TValidationParameter, TSeverityEnum> : IValidationTrigger
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private StringInputValidator<TTargetType, TValidationParameter, TSeverityEnum> Validator { get; }

	private Func<TTargetType> TargetObjectGetter { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	private Action<ValidationError<TSeverityEnum>> PostValidationAction { get; }



	public ValidationTrigger(StringInputValidator<TTargetType, TValidationParameter, TSeverityEnum> validator,
		ValidationEvent validationEvent, Func<TTargetType> targetObjectGetter,
		Func<TValidationParameter> validationParameterGetter, Action<ValidationError<TSeverityEnum>> postValidationAction) {

		Validator = validator;
		TargetObjectGetter = targetObjectGetter;
		ValidationParameterGetter = validationParameterGetter;
		PostValidationAction = postValidationAction;

		validationEvent.Subscribe(EventHandler);
	}



	public void EventHandler() {

		ValidationError<TSeverityEnum>? validationError = Validator.Invoke(TargetObjectGetter.Invoke(), ValidationParameterGetter.Invoke());

		if (validationError is not null) {
			PostValidationAction.Invoke(validationError);
		}
	}

}