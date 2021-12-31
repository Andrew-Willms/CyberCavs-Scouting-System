using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace CCSS_SharedClasses {

	// A custom delegate used as the type for the InputValidator.
	// This is necessary so the InputValidator can have an out paremeter.
	// targetObject is an "out" parameter so this function can change UserInput.TargetObject to a new instance of T if desired.
	public delegate void ValueConverterDelegate<T>(ref T targetObject, Dictionary<string, string> inputStrings, out List<UserInputValidationError> errors);



	public class UserInput<T> {

		#region Properties

		// This is not an auto implemented property because I need to be able to pass the object as an out parameter.
		private T _TargetObject;
		public T TargetObject {
			get => _TargetObject;
			private set => _TargetObject = value;
		}

		// I don't think this needs to be exposed publically in any way since if you know what property you are looking for
		// you should use the "this[string propertyIdentifier]" indexer.
		private Dictionary<string, string> InputStrings { get; init; } = new();

		public bool IsValid { get => ErrorsList.Count == 0; }

		// Since strings are pointers to elements of a pool of strings copying Keys collection is probably safe.
		// Editing, setting, or casting of the output array should not result in the mutation of the InputStrings dictionary.
		private string[] PropertyNames {
			get {
				string[] output = new string[InputStrings.Count];
				InputStrings.Keys.CopyTo(output, 0);
				return output;
			}
		}

		public ValueConverterDelegate<T> ValueConverter { get; private init; }

		// The errors list is "private" instead of "init" because it seems easiser to create a totally
		// new list each time than it is to check each error remove errors from the list.
		// Might be worth checking each error as it comes in to make sure the PropertyIdentifier is valid.
		public List<UserInputValidationError> ErrorsList { get; private set; }

		#endregion Properties

		#region Indexer

		public string this[string propertyIdentifier] {

			// I don't think this will make the InputStrings dictionary editable by outside code.
			get => InputStrings[propertyIdentifier];

			set {
				if (!PropertyNames.Contains(propertyIdentifier)) {
					throw new ArgumentException($"This UserInput object does not contain the propertyIdentifier \"{propertyIdentifier}\"");
				}

				InputStrings[propertyIdentifier] = value;
				ValueConverter(ref _TargetObject, InputStrings, out List<UserInputValidationError> errors);
				ErrorsList = errors;
			}
		}

		#endregion Indexer

		#region Constructors

		// If an array of initial strings is not provided create an array of empty strings of the correct size.
		public UserInput(ValueConverterDelegate<T> valueConverter, string[] propertyNames) :
			this(valueConverter, propertyNames, Enumerable.Repeat("", propertyNames.Length).ToArray()) { }

		// If a dictionary of the property names and initial strings split it into two arrays.
		public UserInput(ValueConverterDelegate<T> valueConverter, Dictionary<string, string> propertyNamesAndInitialStrings) :
			this(valueConverter, propertyNamesAndInitialStrings.Keys.ToArray(), propertyNamesAndInitialStrings.Values.ToArray()) { }

		public UserInput(ValueConverterDelegate<T> valueConverter, string[] propertyNames, string[] initialStrings = null) {

			if (propertyNames.Length != initialStrings.Length) {
				throw new ArgumentException("The length of the propertyNames array and the initialValues array must match.");
			}

			ValueConverter = valueConverter;
			InputStrings = new();

			// Create the necessary entries in the InputStrings dictionary.
			// Since the strings are individually copied editing the source arrays or dictionary should not result in the mutation of internal data.
			for (int i = 0; i < propertyNames.Length; i++) {
				InputStrings.Add(PropertyNames[i], initialStrings[i]);
			}
		}

		#endregion Constructors

		// Try fancy stuff with sub properties by having generic and non generic implementations of a sub class?

	}

}
