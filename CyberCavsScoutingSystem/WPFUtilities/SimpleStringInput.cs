using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WPFUtilities;


// A custom delegate used as the type for the InputValidator.
public delegate (TTargetType, ReadOnlyCollection<ValidationError<TSeverityEnum>>) StringInputConverter<TTargetType, TSeverityEnum>
	(string inputString) where TSeverityEnum : Enum;



// This needs to be an abstract class and not an interface because INotifyPropertyChanged does not work with members defined in an interface.
// IDK if the above line is right.
//public interface IStringInput<TSeverityEnum> where TSeverityEnum : Enum {

//	public string InputString { get; }

//	public bool IsValid { get; }

//	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ErrorsList { get; }

//	// There can't be an implementation here because INotifyPropertyChanged does not work when the implementation is inherited from an interface.
//	public TSeverityEnum ErrorLevel { get; }

//}



public class StringInput<TTargetType, TSeverityEnum> : INotifyPropertyChanged where TSeverityEnum : Enum {

	private TTargetType? _TargetObject;
	public TTargetType? TargetObject {
		get => IsValid ? _TargetObject : default;
		private set => _TargetObject = value;
	}

	private string _InputString = "";
	public string InputString {

		get => _InputString;

		set {
			_InputString = value;
			ConvertInputString();
			OnInputStringChanged();
		}
	}

	public bool IsValid => ErrorsList.Count == 0;

	private StringInputConverter<TTargetType, TSeverityEnum> ValueConverter { get; }

	private ReadOnlyCollection<ValidationError<TSeverityEnum>> _ErrorsList = new List<ValidationError<TSeverityEnum>>().AsReadOnly();
	public ReadOnlyCollection<ValidationError<TSeverityEnum>> ErrorsList {

		get => _ErrorsList;

		private set {
			_ErrorsList = value;
			OnErrorsListChanged();
		}
}

	public TSeverityEnum ErrorLevel {

		get {

			TSeverityEnum? returnValue = ErrorsList.Count > 0 ? ErrorsList.Select(x => x.Severity).Max() : default;

			if (returnValue is null) {
				throw new ArgumentException($"The default value of the TSeverityEnum type parameter \"{typeof(TSeverityEnum)}\" is null.");
			}

			return returnValue;
		}
	}



	public StringInput(StringInputConverter<TTargetType, TSeverityEnum> valueConverter, string initialString = "") {
		ValueConverter = valueConverter;
		InputString = initialString;
	}

	
	
	private void ConvertInputString() {
		(TargetObject, ErrorsList) = ValueConverter(InputString);
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