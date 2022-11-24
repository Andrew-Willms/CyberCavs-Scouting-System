using System.Collections.Generic;
using System;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Collections;

namespace UtilitiesLibrary.Validation.Inputs;



public class MultiInputCreator<TOutput, TSeverity, 
	TInput1, TInput2, TInput3>
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

	private readonly List<OnChangeValidator<TOutput, TSeverity>> OnChangeValidators = new();
	private readonly List<IValidationSet<TOutput, TSeverity>> TriggeredValidators = new();



	public MultiInputCreator<TOutput, TSeverity,
			TInput1, TInput2, TInput3>
		AddOnChangeValidator(OnChangeValidator<TOutput, TSeverity> validator) {

		if (OnChangeValidators.Contains(validator)) {
			throw new ArgumentException("This validator has already been added");
		}

		OnChangeValidators.Add(validator);
		return this;
	}

	public MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3>
		AddTriggeredValidator<TValidationParameter>(TriggeredValidator<TOutput, TValidationParameter, TSeverity> validator,
			Func<TValidationParameter> validationParameterGetter, 
			params ValidationEvent[] validationEvents) {

		ValidationSet<TOutput, TValidationParameter, TSeverity> validationSet = new(validator, validationParameterGetter, validationEvents);
		
		if (TriggeredValidators.Contains(validationSet)) {
			throw new ArgumentException("This validator has already been added");
		}

		TriggeredValidators.Add(validationSet);

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
			OnChangeValidators.ToReadOnly(),
			TriggeredValidators.ToReadOnly());
	}

}



public class MultiInputCreator<TOutput, TSeverity, 
	TInput1, TInput2, TInput3, TInput4>
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

	private readonly List<OnChangeValidator<TOutput, TSeverity>> OnChangeValidators = new();
	private readonly List<IValidationSet<TOutput, TSeverity>> TriggeredValidators = new();



	public MultiInputCreator<TOutput, TSeverity,
			TInput1, TInput2, TInput3, TInput4>
		AddOnChangeValidator(OnChangeValidator<TOutput, TSeverity> validator) {

		if (OnChangeValidators.Contains(validator)) {
			throw new ArgumentException("This validator has already been added");
		}

		OnChangeValidators.Add(validator);
		return this;
	}

	public MultiInputCreator<TOutput, TSeverity, 
			TInput1, TInput2, TInput3, TInput4>
		AddTriggeredValidator<TValidationParameter>(TriggeredValidator<TOutput, TValidationParameter, TSeverity> validator,
			Func<TValidationParameter> validationParameterGetter, 
			params ValidationEvent[] validationEvents) {

		ValidationSet<TOutput, TValidationParameter, TSeverity> validationSet = new(validator, validationParameterGetter, validationEvents);
		
		if (TriggeredValidators.Contains(validationSet)) {
			throw new ArgumentException("This validator has already been added");
		}

		TriggeredValidators.Add(validationSet);

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
			OnChangeValidators.ToReadOnly(),
			TriggeredValidators.ToReadOnly());
	}

}