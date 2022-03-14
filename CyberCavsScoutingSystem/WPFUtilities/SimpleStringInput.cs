using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace WPFUtilities;

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



public class SimpleStringInput<T, TSeverityEnum> : ISimpleStringInput<TSeverityEnum>, INotifyPropertyChanged where TSeverityEnum : Enum {

	#region Properties

	// This is not an auto implemented property because I need to be able to pass the object as an out parameter. (Also I now have logic in get).
	private T? _TargetObject;
	public T? TargetObject => IsValid ? _TargetObject : default;

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

	private SimpleStringInputConverter<T, TSeverityEnum> ValueConverter { get; } // get only properties can be set from the constructor.

	// This property is a ReadOnlyCollection so that every time validation occurs a totally new list is created
	// and the existing list is not edited. This is desirable because it seems easier to create a totally new
	// list each time than it is to check over each error in the existing collection. Since all validation must
	// occur every time the ValueConverter is run there would be no performance benefit from not creating a new
	// collection. In addition using a ReadOnlyCollection saves me from having to implement INotifyCollectionChanged.
	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _ErrorsList = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ErrorsList {

		get => _ErrorsList;

		private set {
			_ErrorsList = value;
			OnErrorsListChanged();
		}
	}

	public TSeverityEnum ErrorLevel => (ErrorsList.Count > 0) ? ErrorsList.Select(x => x.Severity).Max() : default;

	#endregion Properties

	#region Functions

	private void ConvertValue(string inputString) {
		ValueConverter(ref _TargetObject, inputString, out ReadOnlyCollection<ValidationError<TSeverityEnum>> errors);
		ErrorsList = errors;
	}

	// Use this function to force validation. Hopefully it will never need to be used but I will leave it here for now.
	public void Validate() {
		ConvertValue(InputString);
	}

	#endregion Functions

	#region Constructors

	public SimpleStringInput(SimpleStringInputConverter<T, TSeverityEnum> valueConverter, string initialString = "") {
		ValueConverter = valueConverter;
		InputString = initialString;
	}

	#endregion Constructors

	#region PropertyChanged

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	protected void OnErrorsListChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorsList)));
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorLevel)));
	}

	#endregion PropertyChanged

}