using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace CCSS_SharedClasses;

// A custom delegate used as the type for the InputValidator.
// This is necessary so the InputValidator can have ref and out paremeters.
// targetObject is a ref parameter so this function can change StringInput.TargetObject to a new instance of T if desired.
// Consider making the inputStrings Dictionary immutable in some way.
// I guess just pass an empty string for propertyIdentifier if you want the whole object checked?
public delegate void StringInputConverter<T>(ref T targetObject, string propertyIdentifier, Dictionary<string, string> inputStrings,
	out List<StringInputValidationError> errors);



public class StringInput<T> : INotifyPropertyChanged {

	#region Comments

	/*
	 *  I feel that it is necessary for me to list the various requirements of this class to help me make the correct design descisions.
	 *  
	 *  - TargetObjects of some types T may not be mutable and so it is necessary to pass a reference to the TargetObject to the ValueConverter
	 *    (and every potential ValueConverter if I implement per-property ValueConverters) so that ValueConverts can replace TargetObject with a
	 *    new instance of T if necessary.
	 * 
	 *  - It would be nice to be able to directly get errors for a single property (ie an indexer where you specify the property you want errors
	 *    for and it returns the errors belonging to just that proeprty). This will make the presentation logic easier as I am probably going to have
	 *    to generate tooltips and errors messages based on per property errors.
	 *  
	 *  - At some point I may want to separate the data converters from the GameEditingData class and I might want to move them to a static class.
	 *  
	 */

	#endregion Comments

	#region Properties

	// This is not an auto implemented property because I need to be able to pass the object as an out parameter.
	private T _TargetObject = default; // not sure if I should set it to default
	public T TargetObject {

		get => IsValid ? _TargetObject : default;

		private set {
			_TargetObject = value;
			OnPropertyChanged(""); // check to make sure this triggers all of them, even with [CallerMemberName] with the optional property
		}
	}

	// I don't think this needs to be exposed publically in any way since if you know what property you are looking for
	// you should use the "this[string propertyIdentifier]" indexer.
	private Dictionary<string, string> InputStrings { get; init; } = new();

	public bool IsValid { get => ErrorsList.Count == 0; }

	// Since strings are pointers to elements of a pool of strings copying Keys collection is probably safe.
	// Editing, setting, or casting of the output array should not result in the mutation of the InputStrings dictionary.
	public string[] PropertyNames {
		get {
			string[] output = new string[InputStrings.Count];
			InputStrings.Keys.CopyTo(output, 0);
			return output;
		}
	}

	private StringInputConverter<T> ValueConverter { get; init; }

	// The errors list is "private set" instead of "init" because it seems easiser to create a totally
	// new list each time than it is to check each error remove errors from the list.
	// Might be worth checking each error as it comes in to make sure the PropertyIdentifier is valid.
	// Might be worth checking each error as it comes in to make sure the PropertyIdentifier is valid.
	// Might also be worth making the object we pass out from the get accessor is immutable (what the private member is for).
	private List<StringInputValidationError> _ErrorsList;
	public List<StringInputValidationError> ErrorsList { get; private set; }

	#endregion Properties

	#region Indexer


	// Test if changing the name still works with data binding.
	//[IndexerName("test indexer name")]
	public string this[string propertyIdentifier] {

		// I don't think this will make the InputStrings dictionary editable by outside code.
		get => InputStrings[propertyIdentifier];

		set {
			if (!PropertyNames.Contains(propertyIdentifier)) {
				throw new ArgumentException($"This StringInput object does not contain the propertyIdentifier \"{propertyIdentifier}\"");
			}

			InputStrings[propertyIdentifier] = value;
			ValueConverter(ref _TargetObject, propertyIdentifier, InputStrings, out List<StringInputValidationError> errors);
			ErrorsList = errors;

			OnPropertyChanged($"Item[{propertyIdentifier}]"); // Could try Binding.Indexer name and injecting the propertyIdentifier
		}
	}

	#endregion Indexer

	#region Constructors

	// If an array of initial strings is not provided create an array of empty strings of the correct size.
	public StringInput(StringInputConverter<T> valueConverter, string[] propertyNames) :
		this(valueConverter, propertyNames, Enumerable.Repeat("", propertyNames.Length).ToArray()) { }

	// If a dictionary of the property names and initial strings split it into two arrays.
	public StringInput(StringInputConverter<T> valueConverter, Dictionary<string, string> propertyNamesAndInitialStrings) :
		this(valueConverter, propertyNamesAndInitialStrings.Keys.ToArray(), propertyNamesAndInitialStrings.Values.ToArray()) { }

	public StringInput(StringInputConverter<T> valueConverter, string[] propertyNames, string[] initialStrings) {

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

		ValueConverter(ref _TargetObject, "", InputStrings, out List<StringInputValidationError> errors);
		ErrorsList = errors;
	}

	#endregion Constructors

	#region PropertyChanged

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	#endregion PropertyChanged

	// Try fancy stuff with sub properties by having generic and non generic implementations of a sub class?

}