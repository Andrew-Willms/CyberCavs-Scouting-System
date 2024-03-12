using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.SimpleEvent;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation;



public interface IValidationSet<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public IValidator<TSeverity> ToValidator(
		Func<Optional<TOutput>> outputObjectGetter, Event outputObjectChanged,
		Action<IValidator<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction);
}




internal class ValidationSet<TOutput, TSeverity> : IValidationSet<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private bool ValidateOnChange { get; }

	private ValidationRule<TOutput, TSeverity> ValidationRule { get; }
	private Event[] ValidationEvents { get; }

	public ValidationSet(ValidationRule<TOutput, TSeverity> validationRule, bool validateOnChange, params Event[] validationEvents) {

		ValidationRule = validationRule;
		ValidateOnChange = validateOnChange;
		ValidationEvents = validationEvents;
	}

	public IValidator<TSeverity> ToValidator(
		Func<Optional<TOutput>> outputObjectGetter, Event outputObjectChanged,
		Action<IValidator<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction) {

		List<Event> validationEvents = ValidationEvents.ToList();

		if (ValidateOnChange) {
			validationEvents.Add(outputObjectChanged);
		}

		return new Validator<TOutput, TSeverity>(ValidationRule, outputObjectGetter, validationEvents, postValidationAction);
	}

}