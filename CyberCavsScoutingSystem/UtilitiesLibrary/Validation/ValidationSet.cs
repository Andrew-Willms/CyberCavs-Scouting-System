using System;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Validation.Inputs;

namespace UtilitiesLibrary.Validation;



public interface IValidationSet<TOutput, TSeverityEnum> 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(
		Func<Optional<TOutput>> outputObjectGetter,
		IInput<TOutput, TSeverityEnum> validatee,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction);
}



public class ValidationSet<TOutput, TSeverityEnum> : IValidationSet<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TOutput, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }



	public ValidationSet(InputValidator<TOutput, TSeverityEnum> validator, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationEvents = validationEvents;
	}

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(
		Func<Optional<TOutput>> outputObjectGetter,
		IInput<TOutput, TSeverityEnum> validatee,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TOutput, TSeverityEnum>(
			Validator,
			ValidationEvents,
			outputObjectGetter,
			postValidationAction);
	}

}



public class ValidationSet<TOutput, TValidationParameter, TSeverityEnum> : IValidationSet<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private InputValidator<TOutput, TValidationParameter, TSeverityEnum> Validator { get; }

	private ValidationEvent[] ValidationEvents { get; }

	private Func<TValidationParameter> ValidationParameterGetter { get; }



	public ValidationSet(InputValidator<TOutput, TValidationParameter, TSeverityEnum> validator,
		Func<TValidationParameter> validationParameterGetter, params ValidationEvent[] validationEvents) {

		Validator = validator;
		ValidationParameterGetter = validationParameterGetter;
		ValidationEvents = validationEvents;
	}

	public IValidationTrigger<TSeverityEnum> ToValidationTrigger(
		Func<Optional<TOutput>> outputObjectGetter,
		IInput<TOutput, TSeverityEnum> validatee,
		Action<ReadOnlyList<ValidationError<TSeverityEnum>>> postValidationAction) {

		return new ValidationTrigger<TOutput, TValidationParameter, TSeverityEnum>(
			Validator,
			validatee,
			ValidationEvents,
			outputObjectGetter,
			ValidationParameterGetter,
			postValidationAction);
	}

}