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

	public ValidationEvent TargetObjectChanged { get; }

	public void Validate();
}



public interface IInput<out TTargetType, TSeverityEnum> : IInput<TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public TTargetType? TargetObject { get; }
}



public abstract class Input<TTargetType, TSeverityEnum> : IInput<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public abstract TTargetType? TargetObject { get; protected set; }

	protected abstract List<ValidationError<TSeverityEnum>> ValidationErrors { get; }
	public abstract ReadOnlyList<ValidationError<TSeverityEnum>> Errors { get; }
	public abstract TSeverityEnum ErrorLevel { get; }

	public abstract bool IsValid { get; }

	public abstract ValidationEvent TargetObjectChanged { get; }



	protected ReadOnlyList<IValidationTrigger<TSeverityEnum>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TTargetType, TSeverityEnum>> validationSets) {

		TTargetType TargetObjectGetter() {

			if (TargetObject is null) {
				throw new NullReferenceException($"Validators should not be called if {nameof(TargetObject)} is null.");
			}

			return TargetObject;
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