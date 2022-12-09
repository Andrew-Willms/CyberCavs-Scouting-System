using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.SimpleEvent;

namespace UtilitiesLibrary.Validation.Inputs;



public interface IInput<TSeverity> : INotifyPropertyChanged
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public bool IsValid { get; }

	public ReadOnlyList<ValidationError<TSeverity>> Errors { get; }

	public TSeverity ErrorLevel { get; }

	public Event OutputObjectChanged { get; }

	public void Validate();
}



public interface IInput<TOutput, TSeverity> : IInput<TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public Optional<TOutput> OutputObject { get; }
}



public abstract class Input<TOutput, TSeverity> : IInput<TOutput, TSeverity>
	where TSeverity : ValidationErrorSeverityEnum<TSeverity>, IValidationErrorSeverityEnum<TSeverity> {

	public abstract Optional<TOutput> OutputObject { get; protected set; }

	private ReadOnlyKeysDictionary<IValidator<TSeverity>, ReadOnlyList<ValidationError<TSeverity>>> Validators { get; }

	protected ReadOnlyList<ValidationError<TSeverity>> ConversionErrors { get; set; } = ReadOnlyList.Empty;
	protected ReadOnlyList<ValidationError<TSeverity>> ValidationErrors => Validators.Values.Flatten().ToReadOnly();
	public abstract ReadOnlyList<ValidationError<TSeverity>> Errors { get; }

	public TSeverity ConversionErrorLevel => ConversionErrors.Select(x => x.Severity).Max() ?? TSeverity.NoError;
	public TSeverity ValidationErrorLevel => ValidationErrors.Select(x => x.Severity).Max() ?? TSeverity.NoError;
	public TSeverity ErrorLevel => Errors.Select(x => x.Severity).Max() ?? TSeverity.NoError;

	public bool IsValid => ErrorLevel.IsFatal == false;

	public Event OutputObjectChanged { get; } = new();



	protected Input(IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets) {

		Validators = ValidationSetsToTriggers(validationSets).ToReadOnlyKeysDictionary(ReadOnlyList<ValidationError<TSeverity>>.Empty);
	}



	private ReadOnlyList<IValidator<TSeverity>> ValidationSetsToTriggers(
		IEnumerable<IValidationSet<TOutput, TSeverity>> validationSets) {

		Optional<TOutput> OutputObjectGetter() {
			return OutputObject;
		}

		return validationSets.Select(x => x.ToValidator(OutputObjectGetter, OutputObjectChanged, PostValidation)).ToReadOnly();
	}

	protected abstract bool HasValueAndIsNotInvertible(Optional<TOutput> testValue);

	private void PostValidation(IValidator<TSeverity> validator, ReadOnlyList<ValidationError<TSeverity>> validationError) {

		Validators[validator] = validationError;
		OnErrorsChanged();
	}

	public abstract void Validate();
	public event PropertyChangedEventHandler? PropertyChanged;

	protected abstract void OnErrorsChanged();

}