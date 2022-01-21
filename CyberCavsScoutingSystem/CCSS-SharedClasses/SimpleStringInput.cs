using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CCSS_SharedClasses {

	// A custom delegate used as the type for the InputValidator.
	// This is necessary so the InputValidator can have ref and out paremeters.
	// targetObject is a ref parameter so this function can change SimpleStringInput.TargetObject to a new instance of T if desired.
	public delegate void SimpleValueConverterDelegate<T>(ref T targetObject, string inputString, out List<StringInputValidationError> errors);

	public class SimpleStringInput<T> : INotifyPropertyChanged {

		#region Properties

		// This is not an auto implemented property because I need to be able to pass the object as an out parameter.
		private T _TargetObject = default; // not sure if I should set it to default
		public T TargetObject {

			get => IsValid ? _TargetObject : default;

			private set {
				_TargetObject = value;
				OnInputStringChanged();
			}
		}

		private string _InputString = "";
		public string InputString {

			get => _InputString;

			set {
				_InputString = value;
				ConvertValue(value);
				OnInputStringChanged();
			}
		}

		public bool IsValid { get => ErrorsList.Count == 0; }

		private SimpleValueConverterDelegate<T> ValueConverter { get; init; }



		// !!!!!!!!! ErrorsList binding? PropertyChanged???? Maybe make it an unordered collection like HashSet????? Maybe not.

		// The errors list is "private set" instead of "init" because it seems easiser to create a totally
		// new list each time than it is to check each error remove errors from the list.
		// Might be worth checking each error as it comes in to make sure the PropertyIdentifier is valid.
		private List<StringInputValidationError> _ErrorsList;
		public List<StringInputValidationError> ErrorsList {

			get => _ErrorsList; // Find a way of guaranteeing the immutability of this.
			
			private set {
				_ErrorsList = value;
				OnErrorsListChanged();
			}
		}

		#endregion Properties

		#region Functions

		private void ConvertValue(string inputString) {
			ValueConverter(ref _TargetObject, inputString, out List<StringInputValidationError> errors);
			ErrorsList = errors;
		}

		#endregion Functions

		#region Constructors

		public SimpleStringInput(SimpleValueConverterDelegate<T> valueConverter, string initialString = "") {
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

}
