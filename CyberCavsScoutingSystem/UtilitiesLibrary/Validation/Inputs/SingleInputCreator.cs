using System.Collections.Generic;
using System;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Collections;

namespace UtilitiesLibrary.Validation.Inputs;



public class SingleInputCreator<TOutput, TInput, TSeverity>
	where TInput : IEquatable<TInput>
	where TOutput : IEquatable<TOutput>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {



	public required InputConverter<TOutput, TInput, TSeverity> Converter { get; init; }
	public required InputInverter<TOutput, TInput, TSeverity> Inverter { get; init; }
	public required TInput InitialInput { get; init; }
	private readonly List<ValidationSet<TOutput, TSeverity>> ValidationSets = new();



	public SingleInputCreator<TOutput, TInput, TSeverity> AddValidationRule(
		ValidationRule<TOutput, TSeverity> validationRule, bool validateOnChange = true, params ValidationEvent[] validationEvents) {

		ValidationSets.Add(new(validationRule, validateOnChange, validationEvents));
		return this;
	}

	public SingleInputCreator<TOutput, TInput, TSeverity> AddValidationRule<TValidationParameter>(
		ValidationRule<TOutput, TValidationParameter, TSeverity> validator, Func<TValidationParameter> validationParameterGetter, 
		bool validateOnChange = true, params ValidationEvent[] validationEvents) {

		ReadOnlyList<ValidationError<TSeverity>> SimplifiedValidationRule(TOutput outputObject) {
			return validator.Invoke(outputObject, validationParameterGetter.Invoke());
		}

		ValidationSets.Add(new(SimplifiedValidationRule, validateOnChange, validationEvents));
		return this;
	}

	public SingleInput<TOutput, TInput, TSeverity> CreateSingleInput() {

		return new(
			converter: Converter,
			inverter: Inverter,
			initialInput: InitialInput,
			validationSets: ValidationSets.ToReadOnly());
	}

}