using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation;



public interface IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public void EventHandler();

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator();
}



internal class ValidationTrigger<TOutput, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidatorErrorsList<TOutput, TSeverityEnum> Validator { get; }

	private Func<TOutput> OutputObjectGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(InputValidatorErrorsList<TOutput, TSeverityEnum> validator,
		IEnumerable<ValidationEvent> validationEvents, Func<TOutput> outputObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		OutputObjectGetter = outputObjectGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	public void EventHandler() {

		ReadOnlyList<ValidationError<TSeverityEnum>> validationError = Validator.Invoke(OutputObjectGetter.Invoke());

		if (validationError.Any()) {
			PostValidationAction.Invoke(validationError);
		}
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		return Validator.Invoke(OutputObjectGetter.Invoke());
	}

}



internal class ValidationTrigger<TOutput, TValidationParameter, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum> Validator { get; }

	private Func<TOutput> OutputObjectGetter { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(InputValidatorErrorsList<TOutput, TValidationParameter, TSeverityEnum> validator,
		IEnumerable<ValidationEvent> validationEvents, Func<TOutput> outputObjectGetter,
		Func<TValidationParameter> validationParameterGetter, Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		OutputObjectGetter = outputObjectGetter;
		ValidationParameterGetter = validationParameterGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	public void EventHandler() {

		ReadOnlyList<ValidationError<TSeverityEnum>> validationError = Validator.Invoke(OutputObjectGetter.Invoke(), ValidationParameterGetter.Invoke());

		if (validationError.Any()) {
			PostValidationAction.Invoke(validationError);
		}
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		return Validator.Invoke(OutputObjectGetter.Invoke(), ValidationParameterGetter.Invoke());
	}

}