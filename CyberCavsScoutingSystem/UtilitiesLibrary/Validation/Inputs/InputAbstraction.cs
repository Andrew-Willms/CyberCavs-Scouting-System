using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using UtilitiesLibrary.Extensions;
using UtilitiesLibrary.Validation.Errors;

namespace UtilitiesLibrary.Validation.Inputs;



public interface IInput<TSeverityEnum> : INotifyPropertyChanged
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public bool IsValid { get; }

	public ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }

	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
	public TSeverityEnum ErrorLevel { get; }

	public ValidationEvent OutputObjectChanged { get; }

	public void Validate();
}



public interface IInput<TOutput, TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public Optional<TOutput> OutputObject { get; }

	public void SetOutputValue(TOutput value);
}



public abstract class Input<TOutput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public abstract Optional<TOutput> OutputObject { get; protected set; }

	protected abstract List<ValidationError<TSeverityEnum>> ValidationErrors { get; }
	public abstract ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }
	public abstract TSeverityEnum ErrorLevel { get; }

	public abstract bool IsValid { get; }

	public abstract ValidationEvent OutputObjectChanged { get; }



	protected ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TOutput, TSeverityEnum>> validationSets) {

		TOutput TargetObjectGetter() {

			if (!OutputObject.HasValue) {
				throw new NullReferenceException($"Validators should not be called if {nameof(OutputObject)} is null.");
			}

			return OutputObject.Value;
		}

		return validationSets.Select(x => x.ToValidationTrigger(TargetObjectGetter, PostValidation)).ToReadOnly();
	}

	public abstract void SetOutputValue(TOutput value);

	private void PostValidation(ReadOnlyList<ValidationError<TSeverityEnum>> validationError) {
		ValidationErrors.AddRange(validationError);
		OnErrorsChanged();
	}

	public abstract void Validate();
	public abstract event PropertyChangedEventHandler? PropertyChanged;

	protected abstract void OnErrorsChanged();
}