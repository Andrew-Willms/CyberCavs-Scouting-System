using System;
using System.Collections.Generic;
using UtilitiesLibrary.Extensions;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation;



public interface IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator();
}



internal class ValidationTrigger<TOutput, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TOutput, TSeverityEnum> Validator { get; }

	private Func<Optional<TOutput>> OutputObjectGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(
		InputValidator<TOutput, TSeverityEnum> validator,
		IEnumerable<ValidationEvent> validationEvents,
		Func<Optional<TOutput>> outputObjectGetter,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		OutputObjectGetter = outputObjectGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	private void EventHandler() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		List<ValidationError<TSeverityEnum>> validationError = new();

		if (output.HasValue) {
			validationError.AddRange(Validator.Invoke(output.Value));
		}

		PostValidationAction.Invoke(validationError.ToReadOnly());
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		return !output.HasValue
			? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty
			: Validator.Invoke(output.Value);
	}

}



internal class ValidationTrigger<TOutput, TValidationParameter, TSeverityEnum> : IValidationTrigger<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TOutput, TValidationParameter, TSeverityEnum> Validator { get; }

	private Func<Optional<TOutput>> OutputObjectGetter { get; }

	private IInput<TOutput, TSeverityEnum> Validatee { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	private Action<ReadOnlyList<ValidationError<TSeverityEnum>>> PostValidationAction { get; }



	public ValidationTrigger(
		InputValidator<TOutput, TValidationParameter, TSeverityEnum> validator,
		IInput<TOutput, TSeverityEnum> validatee,
		IEnumerable<ValidationEvent> validationEvents,
		Func<Optional<TOutput>> outputObjectGetter,
		Func<TValidationParameter> validationParameterGetter, Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		Validator = validator;
		Validatee = validatee;
		OutputObjectGetter = outputObjectGetter;
		ValidationParameterGetter = validationParameterGetter;
		PostValidationAction = postValidationAction;

		foreach (ValidationEvent validationEvent in validationEvents) {
			validationEvent.Subscribe(EventHandler);
		}
	}



	private void EventHandler() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		List<ValidationError<TSeverityEnum>> validationError = new();

		if (output.HasValue) {
			validationError.AddRange(Validator.Invoke(output.Value, Validatee, ValidationParameterGetter.Invoke()));
		}

		PostValidationAction.Invoke(validationError.ToReadOnly());
	}

	public ReadOnlyList<ValidationError<TSeverityEnum>> InvokeValidator() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		return !output.HasValue
			? ReadOnlyList<ValidationError<TSeverityEnum>>.Empty
			: Validator.Invoke(output.Value, Validatee, ValidationParameterGetter.Invoke());
	}

}