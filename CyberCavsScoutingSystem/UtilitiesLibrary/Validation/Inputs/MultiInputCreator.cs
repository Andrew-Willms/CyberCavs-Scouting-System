using System.Collections.Generic;
using System;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Collections;

namespace UtilitiesLibrary.Validation.Inputs;



public abstract class MultiInputCreator<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	internal readonly List<ValidationSet<TOutput, TSeverity>> ValidationSets = new();

	protected void AddValidationRule(
		ValidationRule<TOutput, TSeverity> validationRule, bool validateOnChange = true, params Event[] validationEvents) {

		ValidationSets.Add(new(validationRule, validateOnChange, validationEvents));
	}

	protected void AddValidationRule<TValidationParameter>(
		ValidationRule<TOutput, TValidationParameter, TSeverity> validator, Func<TValidationParameter> validationParameterGetter, 
		bool validateOnChange = true, params Event[] validationEvents) {

		ReadOnlyList<ValidationError<TSeverity>> SimplifiedValidationRule(TOutput outputObject) {
			return validator.Invoke(outputObject, validationParameterGetter.Invoke());
		}

		ValidationSets.Add(new(SimplifiedValidationRule, validateOnChange, validationEvents));
	}

}



public class MultiInputCreator<TOutput, TSeverity, 
	TInput1, TInput2, TInput3> : 
	MultiInputCreator<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public required IInput<TInput1, TSeverity> InputComponent1 { get; init; }
	public required IInput<TInput2, TSeverity> InputComponent2 { get; init; }
	public required IInput<TInput3, TSeverity> InputComponent3 { get; init; }

	public required InputConverter<TOutput, 
		(TInput1, TInput2, TInput3),
		TSeverity> Converter { get; init; }

	public required InputInverter<TOutput,
		(TInput1, TInput2, TInput3),
		TSeverity> Inverter { get; init; }



	public new MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3>
		AddValidationRule(ValidationRule<TOutput, TSeverity> validationRule, bool validateOnChange = true, params Event[] validationEvents) {

		base.AddValidationRule(validationRule, validateOnChange, validationEvents);
		return this;
	}

	public new MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3>
		AddValidationRule<TValidationParameter>(ValidationRule<TOutput, TValidationParameter, TSeverity> validator,
			Func<TValidationParameter> validationParameterGetter, bool validateOnChange = true, params Event[] validationEvents) {

		base.AddValidationRule(validator, validationParameterGetter, validateOnChange, validationEvents);
		return this;
	}



	public MultiInput<TOutput, TSeverity, 
			TInput1, TInput2, TInput3>
		CreateMultiInput() {

		return new(
			Converter,
			Inverter,
			InputComponent1,
			InputComponent2,
			InputComponent3,
			ValidationSets.ToReadOnly());
	}

}



public class MultiInputCreator<TOutput, TSeverity, 
	TInput1, TInput2, TInput3, TInput4> : 
	MultiInputCreator<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public required IInput<TInput1, TSeverity> InputComponent1 { get; init; }
	public required IInput<TInput2, TSeverity> InputComponent2 { get; init; }
	public required IInput<TInput3, TSeverity> InputComponent3 { get; init; }
	public required IInput<TInput4, TSeverity> InputComponent4 { get; init; }

	public required InputConverter<TOutput, 
		(TInput1, TInput2, TInput3, TInput4),
		TSeverity> Converter { get; init; }

	public required InputInverter<TOutput,
		(TInput1, TInput2, TInput3, TInput4),
		TSeverity> Inverter { get; init; }



	public new MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3, TInput4>
		AddValidationRule(ValidationRule<TOutput, TSeverity> validationRule, bool validateOnChange = true, params Event[] validationEvents) {

		base.AddValidationRule(validationRule, validateOnChange, validationEvents);
		return this;
	}

	public new MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3, TInput4>
		AddValidationRule<TValidationParameter>(ValidationRule<TOutput, TValidationParameter, TSeverity> validator,
			Func<TValidationParameter> validationParameterGetter, bool validateOnChange = true, params Event[] validationEvents) {

		base.AddValidationRule(validator, validationParameterGetter, validateOnChange, validationEvents);
		return this;
	}



	public MultiInput<TOutput, TSeverity, 
			TInput1, TInput2, TInput3, TInput4>
		CreateMultiInput() {

		return new(
			Converter,
			Inverter,
			InputComponent1,
			InputComponent2,
			InputComponent3,
			InputComponent4,
			ValidationSets.ToReadOnly());
	}

}