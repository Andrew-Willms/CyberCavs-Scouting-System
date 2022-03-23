using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities;

//TODO: change this to return a tuple instead of having targetObject be a ref parameter and the dictionary be an out parameter.
// A custom delegate used as the type for the InputValidator.
// This is necessary so the InputValidator can have ref and out parameters.
// targetObject is a ref parameter so this function can change SimpleStringInput.TargetObject to a new instance of T if desired.
public delegate void SimpleStringInputConverter<T, TSeverityEnum>
	(ref T? targetObject, string inputString, out ReadOnlyCollection<ValidationError<TSeverityEnum>> errors) where TSeverityEnum : Enum;



// This needs to be an abstract class and not an interface because INotifyPropertyChanged does not work with members defined in an interface
public interface ISimpleStringInput<TSeverityEnum> where TSeverityEnum : Enum {

	public string InputString { get; }

	public bool IsValid { get; }

	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ErrorsList { get; }

	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
	public TSeverityEnum ErrorLevel { get; }

}



// TODO: rename this to StringInput (the other one will be MultiStringInput)
public class SimpleStringInput<TTargetType, TSeverityEnum> : ISimpleStringInput<TSeverityEnum>, INotifyPropertyChanged where TSeverityEnum : Enum {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject => IsValid ? _TargetObject : default;

	private string _InputString = "";
	public string InputString {

		get => _InputString;

		set {
			// Even if the strings match, validate the input. I think this is the behavior I want.
			// However, if the string is the same, the View doesn't need to be updated.
			if (_InputString == value) {
				ConvertValue(value);

			} else {
				_InputString = value;
				ConvertValue(value);
				OnInputStringChanged();
			}
		}
	}

	public bool IsValid => ErrorsList.Count == 0;

	private SimpleStringInputConverter<TTargetType, TSeverityEnum> ValueConverter { get; }

	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _ErrorsList = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ErrorsList {

		get => _ErrorsList;

		private set {
			_ErrorsList = value;
			OnErrorsListChanged();
		}
	}

	public TSeverityEnum ErrorLevel => (ErrorsList.Count > 0) ? ErrorsList.Select(x => x.Severity).Max() : default;



	public SimpleStringInput(SimpleStringInputConverter<TTargetType, TSeverityEnum> valueConverter, string initialString = "") {
		ValueConverter = valueConverter;
		InputString = initialString;
	}

	
	
	private void ConvertValue(string inputString) {
		ValueConverter(ref _TargetObject, inputString, out ReadOnlyCollection<ValidationError<TSeverityEnum>> errors);
		ErrorsList = errors;
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	protected void OnErrorsListChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorsList)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
	}

}