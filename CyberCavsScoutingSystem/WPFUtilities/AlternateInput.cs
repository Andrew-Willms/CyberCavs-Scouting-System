using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities;



// A custom delegate used as the type for the InputValidator.
public delegate(TTargetType, ReadOnlyCollection<ValidationError<TSeverityEnum>>) AlternateInputConverter<TTargetType, TSeverityEnum>
	(string inputString) where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

// A custom delegate used as the type for the InputValidator.
public delegate ReadOnlyCollection<ValidationError<TSeverityEnum>> AlternateInputValidator<in TTargetType, TSeverityEnum>
	(TTargetType value) where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public interface IAlternateInput<TSeverityEnum> where TSeverityEnum :
	ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public string InputString { get; set; }

	public bool IsValid { get; }

	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ValidationErrors { get; }

	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
	public TSeverityEnum ErrorLevel { get; }

	public void ValidateInput();

	public event PropertyChangedEventHandler? PropertyChanged;

}



public class AlternateInput<TTargetType, TSeverityEnum> : IAlternateInput<TSeverityEnum>, INotifyPropertyChanged
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject {

		// TODO: .Net 7.0 remove backing field
		get => IsValid ? _TargetObject : default;

		private set {
			_TargetObject = value;
			OnTargetObjectChanged();
		}
	}

	private string _InputString = "";
	public string InputString {

		// TODO: .Net 7.0 remove backing field
		get => _InputString;

		set {
			_InputString = value;
			ValidateInput();
			OnInputStringChanged();
		}
	}

	private AlternateInputConverter<TTargetType, TSeverityEnum> Converter { get; }

	private ReadOnlyCollection<AlternateInputValidator<TTargetType, TSeverityEnum>> Validators { get; }

	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _ValidationErrors = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ValidationErrors {

		// TODO: .Net 7.0 remove backing field
		get => _ValidationErrors;

		private set {
			_ValidationErrors = value;
			OnErrorsChanged();
		}
	}

	public bool IsValid => ErrorLevel.IsFatal == false;

	public TSeverityEnum ErrorLevel {

		get {

			TSeverityEnum? returnValue = ValidationErrors.Any()
				? ValidationErrors.Select(x => x.Severity).Max()
				: TSeverityEnum.NoError;

			if (returnValue is null) {
				throw new ArgumentException("Something here was null. I'm not sure how.");
			}

			return returnValue;
		}
	}



	public AlternateInput(AlternateInputConverter<TTargetType, TSeverityEnum> converter, string initialString,
		params AlternateInputValidator<TTargetType, TSeverityEnum>[] validators) {

		Converter = converter;

		Validators = validators.ToList().AsReadOnly();

		InputString = initialString;
	}

	
	
	public void ValidateInput() {

		(TargetObject, ValidationErrors) = Converter(InputString);

		if (IsValid == false) {
			return;
		}

		foreach (AlternateInputValidator<TTargetType, TSeverityEnum> validator in Validators) {

			ReadOnlyCollection<ValidationError<TSeverityEnum>> newErrors = validator(TargetObject);

			// make an extension method GetMaxErrorLevel() later but use this for now

			ValidationErrors = ValidationErrors.Concat(newErrors).ToList().AsReadOnly();

			if (IsValid == false) {
				return;
			}
		}

	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnTargetObjectChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetObject)));
	}

	private void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	private void OnErrorsChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationErrors)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
	}

}




public class TestInput<TTargetType, TSeverityEnum>
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {




}