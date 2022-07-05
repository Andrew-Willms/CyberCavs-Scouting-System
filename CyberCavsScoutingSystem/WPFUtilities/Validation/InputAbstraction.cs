using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using WPFUtilities.Extensions;

namespace WPFUtilities.Validation;



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

	public TOutput? OutputObject { get; set; }
}



public abstract class Input<TOutput, TSeverityEnum> : IInput<TOutput, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public abstract TOutput? OutputObject { get; set; }

	protected abstract List<ValidationError<TSeverityEnum>> ValidationErrors { get; }
	public abstract ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }
	public abstract TSeverityEnum ErrorLevel { get; }

	public abstract bool IsValid { get; }

	public abstract ValidationEvent OutputObjectChanged { get; }



	protected ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TOutput, TSeverityEnum>> validationSets) {

		TOutput TargetObjectGetter() {

			if (OutputObject is null) {
				throw new NullReferenceException($"Validators should not be called if {nameof(OutputObject)} is null.");
			}

			return OutputObject;
		}

		return validationSets.Select(x => x.ToValidationTrigger(TargetObjectGetter, PostValidation)).ToReadOnly();
	}

	private void PostValidation(ReadOnlyList<ValidationError<TSeverityEnum>> validationError) {
		ValidationErrors.AddRange(validationError);
		OnErrorsChanged();
	}

	public abstract void Validate();
	public abstract event PropertyChangedEventHandler? PropertyChanged;

	protected abstract void OnErrorsChanged();
}