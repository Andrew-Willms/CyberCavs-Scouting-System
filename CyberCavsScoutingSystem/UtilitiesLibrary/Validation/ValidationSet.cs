using System;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation;



public interface IValidationSet<TOutput, TSeverity> 
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public IValidationTrigger<TSeverity> ToValidationTrigger(
		Func<Optional<TOutput>> outputObjectGetter,
		IInput<TOutput, TSeverity> validatee,
		Action<IValidationTrigger<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction);
}



internal class ValidationSet<TOutput, TValidationParameter, TSeverity> : IValidationSet<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	private TriggeredValidator<TOutput, TValidationParameter, TSeverity> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }



	public ValidationSet(TriggeredValidator<TOutput, TValidationParameter, TSeverity> validator,
		Func<TValidationParameter> validationParameterGetter, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationParameterGetter = validationParameterGetter;
		ValidationEvents = validationEvents;
	}

	public IValidationTrigger<TSeverity> ToValidationTrigger(
		Func<Optional<TOutput>> outputObjectGetter,
		IInput<TOutput, TSeverity> validatee,
		Action<IValidationTrigger<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> postValidationAction) {

		return new ValidationTrigger<TOutput, TValidationParameter, TSeverity>(
			Validator,
			validatee,
			ValidationEvents,
			outputObjectGetter,
			ValidationParameterGetter,
			postValidationAction);
	}

}