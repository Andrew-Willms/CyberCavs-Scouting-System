using System;
using System.Linq;
using WPFUtilities.Validation.Delegates;
using WPFUtilities.Validation.Errors;

namespace WPFUtilities.Validation;



public interface IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public void EventHandler();

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator();
}



public class ValidationTrigger<TTargetType, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TTargetType, TSeverityEnum> Validator { get; }

	private Func<TTargetType> TargetObjectGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(InputValidator<TTargetType, TSeverityEnum> validator, ValidationEvent[] validationEvents,
		Func<TTargetType> targetObjectGetter, Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		TargetObjectGetter = targetObjectGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	public void EventHandler() {

		ReadOnlyList<ValidationError<TSeverityEnum>> validationError = Validator.Invoke(TargetObjectGetter.Invoke());

		if (validationError.Any()) {
			PostValidationAction.Invoke(validationError);
		}
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		return Validator.Invoke(TargetObjectGetter.Invoke());
	}

}



public class ValidationTrigger<TTargetType, TValidationParameter, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TTargetType, TValidationParameter, TSeverityEnum> Validator { get; }

	private Func<TTargetType> TargetObjectGetter { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(InputValidator<TTargetType, TValidationParameter, TSeverityEnum> validator,
		ValidationEvent[] validationEvents, Func<TTargetType> targetObjectGetter,
		Func<TValidationParameter> validationParameterGetter, Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		TargetObjectGetter = targetObjectGetter;
		ValidationParameterGetter = validationParameterGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	public void EventHandler() {

		ReadOnlyList<ValidationError<TSeverityEnum>> validationError = Validator.Invoke(TargetObjectGetter.Invoke(), ValidationParameterGetter.Invoke());

		if (validationError.Any()) {
			PostValidationAction.Invoke(validationError);
		}
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		return Validator.Invoke(TargetObjectGetter.Invoke(), ValidationParameterGetter.Invoke());
	}

}