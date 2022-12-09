using System;
using System.Collections.Generic;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation;



public interface IValidator<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

}



internal class Validator<TOutput, TSeverity> : IValidator<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private ValidationRule<TOutput, TSeverity> ValidationRule { get; }

	private Func<Optional<TOutput>> OutputObjectGetter { get; }

	private Action<IValidator<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> PostValidationAction { get; }



	public Validator(
		ValidationRule<TOutput, TSeverity> validationRule,
		Func<Optional<TOutput>> outputObjectGetter,
		IEnumerable<Event> validationEvents,
		Action<IValidator<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction) {

		ValidationRule = validationRule;
		OutputObjectGetter = outputObjectGetter;
		PostValidationAction = postValidationAction;

		validationEvents.Foreach(x => x.Subscribe(EventHandler));
	}



	private void EventHandler() {

		Optional<TOutput> output = OutputObjectGetter.Invoke();

		List<ValidationError<TSeverity>> validationErrors = new();

		if (output.HasValue) {
			validationErrors.AddRange(ValidationRule.Invoke(output.Value));
		}

		PostValidationAction.Invoke(this, validationErrors.ToReadOnly());
	}

}