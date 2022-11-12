﻿using System.Collections.Generic;
using System;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation.Delegates;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Inputs; 



public class SingleInputCreator<TOutput, TInput, TSeverity>
	where TInput : IEquatable<TInput>
	where TOutput : IEquatable<TOutput>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {



	public required InputConverter<TOutput, TInput, TSeverity> Converter { get; init; }
	public required InputInverter<TOutput, TInput, TSeverity> Inverter { get; init; }
	public required TInput InitialInput { get; init; }

	private readonly List<OnChangeValidator<TOutput, TSeverity>> OnChangeValidators = new();
	private readonly List<IValidationSet<TOutput, TSeverity>> TriggeredValidators = new();

	public SingleInputCreator<TOutput, TInput, TSeverity> AddOnChangeValidator(
		OnChangeValidator<TOutput, TSeverity> validator) {

		if (OnChangeValidators.Contains(validator)) {
			throw new ArgumentException("This validator has already been added");
		}

		OnChangeValidators.Add(validator);
		return this;
	}

	public SingleInputCreator<TOutput, TInput, TSeverity> AddTriggeredValidator<TValidationParameter>(
		TriggeredValidator<TOutput, TValidationParameter, TSeverity> validator,
		Func<TValidationParameter> validationParameterGetter,
		params ValidationEvent[] validationEvents) {

		ValidationSet<TOutput, TValidationParameter, TSeverity> validationSet = new(validator, validationParameterGetter, validationEvents);
		
		if (TriggeredValidators.Contains(validationSet)) {
			throw new ArgumentException("This validator has already been added");
		}

		TriggeredValidators.Add(validationSet);

		return this;
	}

	public SingleInput<TOutput, TInput, TSeverity> CreateSingleInput() {

		return new(
			Converter,
			Inverter,
			InitialInput,
			OnChangeValidators.ToReadOnly(),
			TriggeredValidators.ToReadOnly());
	}

}