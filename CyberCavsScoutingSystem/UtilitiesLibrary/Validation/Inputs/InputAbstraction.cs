using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation.Delegates;

namespace UtilitiesLibrary.Validation.Inputs;



public interface IInput<TSeverity> : INotifyPropertyChanged
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public bool IsValid { get; }

	public ReadOnlyList<ValidationError<TSeverity>> Errors { get; }

	public TSeverity ErrorLevel { get; }

	public ValidationEvent OutputObjectChanged { get; }

	public void Validate();
}



public interface IInput<TOutput, TSeverity> : IInput<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public Optional<TOutput> OutputObject { get; }
}



public abstract class Input<TOutput, TSeverity> : IInput<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public abstract Optional<TOutput> OutputObject { get; protected set; }

	protected ReadOnlyKeysDictionary<OnChangeValidator<TOutput, TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> OnChangedValidation { get; }
	protected ReadOnlyKeysDictionary<IValidationTrigger<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> TriggeredValidation { get; }

	public ReadOnlyList<ValidationError<TSeverity>> ValidationErrors => OnChangedValidation.Values.Flatten().AppendRanges(TriggeredValidation.Values).ToReadOnly();
	public abstract ReadOnlyList<ValidationError<TSeverity>> Errors { get; }
	public abstract TSeverity ErrorLevel { get; }

	public abstract bool IsValid { get; }

	public abstract ValidationEvent OutputObjectChanged { get; }



	protected Input(IEnumerable<OnChangeValidator<TOutput, TSeverity>> onChangeValidators,
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets) {

		OnChangedValidation = onChangeValidators.ToReadOnlyKeysDictionary(ReadOnlyList<ValidationError<TSeverity>>.Empty);

		TriggeredValidation = ValidationSetsToTriggers(validationSets).ToReadOnlyKeysDictionary(ReadOnlyList<ValidationError<TSeverity>>.Empty);
	}



	protected ReadOnlyList<IValidationTrigger<TSeverity>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets) {

		Optional<TOutput> OutputObjectGetter() {
			return OutputObject;
		}

		return validationSets.Select(x => x.ToValidationTrigger(OutputObjectGetter, this, PostValidation)).ToReadOnly();
	}

	protected abstract bool HasValueAndIsNotInvertible(Optional<TOutput> testValue);

	private void PostValidation(IValidationTrigger<TSeverity> validationTrigger, ReadOnlyList<ValidationError<TSeverity>> validationError) {

		TriggeredValidation[validationTrigger] = validationError;
		OnErrorsChanged();
	}

	public abstract void Validate();
	public abstract event PropertyChangedEventHandler? PropertyChanged;

	protected abstract void OnErrorsChanged();
}