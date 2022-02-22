using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CCSS_SharedClasses;

// A custom delegate used as the type for the InputValidator.
// This is necessary so the InputValidator can have ref and out parameters.
// targetObject is a ref parameter so this function can change SimpleStringInput.TargetObject to a new instance of T if desired.
public delegate void SimpleStringInputConverter<T>(ref T targetObject, string inputString, out ReadOnlyCollection<StringInputValidationError> errors);

public interface ISimpleStringInput {

	public string InputString { get; }

	public bool IsValid { get; }

	public ReadOnlyCollection<StringInputValidationError> ErrorsList { get; }

	public int GetErrorLevel => ErrorsList.Select(x => (int)x.Severity).Max();
}

public class SimpleStringInput<T> : ISimpleStringInput, INotifyPropertyChanged {

	#region Properties

	// This is not an auto implemented property because I need to be able to pass the object as an out parameter. (Also I now have logic in get).
	private T _TargetObject;
	public T TargetObject => IsValid ? _TargetObject : default;

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

	private SimpleStringInputConverter<T> ValueConverter { get; } // get only properties can be set from the constructor.

	// This property is a ReadOnlyCollection so that every time validation occurs a totally new list is created
	// and the existing list is not edited. This is desirable because it seems easier to create a totally new
	// list each time than it is to check over each error in the existing collection. Since all validation must
	// occur every time the ValueConverter is run there would be no performance benefit from not creating a new
	// collection. In addition using a ReadOnlyCollection saves me from having to implement INotifyCollectionChanged.
	private ReadOnlyCollection<StringInputValidationError> _ErrorsList;
	public ReadOnlyCollection<StringInputValidationError> ErrorsList {

		get => _ErrorsList;

		private set {
			_ErrorsList = value;
			OnErrorsListChanged();
		}
	}

	#endregion Properties

	#region Functions

	private void ConvertValue(string inputString) {
		ValueConverter(ref _TargetObject, inputString, out ReadOnlyCollection<StringInputValidationError> errors);
		ErrorsList = errors;
	}

	// Use this function to force validation. Hopefully it will never need to be used but I will leave it here for now.
	public void Validate() {
		ConvertValue(InputString);
	}

	#endregion Functions

	#region Constructors

	public SimpleStringInput(SimpleStringInputConverter<T> valueConverter, string initialString = "") {
		ValueConverter = valueConverter;
		InputString = initialString;
	}

	#endregion Constructors

	#region PropertyChanged

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnInputStringChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputString)));
	}

	protected void OnErrorsListChanged() {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorsList)));
	}

	#endregion PropertyChanged

}