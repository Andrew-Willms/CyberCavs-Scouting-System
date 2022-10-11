using System;
using System.Collections.Generic;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation;



public interface IValidationTrigger<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public ReadOnlyList<ValidationError<TSeverity>> InvokeValidator();
}



internal class ValidationTrigger<TOutput, TValidationParameter, TSeverity> : IValidationTrigger<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private TriggeredValidator<TOutput, TValidationParameter, TSeverity> Validator { get; }

	private Func<Optional<TOutput>> OutputObjectGetter { get; }

	private IInput<TOutput, TSeverity> Validatee { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }

	private Action<IValidationTrigger<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> PostValidationAction { get; }



	public ValidationTrigger(
		TriggeredValidator<TOutput, TValidationParameter, TSeverity> validator,
		IInput<TOutput, TSeverity> validatee,
		IEnumerable<ValidationEvent> validationEvents,
		Func<Optional<TOutput>> outputObjectGetter,
		Func<TValidationParameter> validationParameterGetter,
		Action<IValidationTrigger<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction) {

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

		List<ValidationError<TSeverity>> validationError = new();

		if (output.HasValue) {
			validationError.AddRange(Validator.Invoke(output.Value, Validatee, ValidationParameterGetter.Invoke()));
		}

		PostValidationAction.Invoke(this, validationError.ToReadOnly());
	}

	public ReadOnlyList<ValidationError<TSeverity>> InvokeValidator() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		return !output.HasValue
			? ReadOnlyList<ValidationError<TSeverity>>.Empty
			: Validator.Invoke(output.Value, Validatee, ValidationParameterGetter.Invoke());
	}

}