using System;
using System.ComponentModel;
using System.Linq;
using CCSSDomain.GameSpecification;
using ExhaustiveMatching;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace CCSSDomain.DataCollectors;



[Closed(typeof(BooleanDataField), typeof(TextDataField), typeof(IntegerDataField), typeof(SelectionDataField))]
public abstract class DataField : INotifyPropertyChanged {

	public DataFieldSpec DataFieldSpec { get; }

	public string Name => DataFieldSpec.Name;

	public readonly Event OnValueChange = new();

	protected DataField(DataFieldSpec dataFieldSpec) {
		DataFieldSpec = dataFieldSpec;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

	public abstract void AddErrorsToCollection(Action<string> addErrorString);

}



public class BooleanDataField : DataField {

	public BooleanDataFieldSpec BooleanDataFieldSpec { get; }

	public bool Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
		}
	}

	public BooleanDataField(BooleanDataFieldSpec dataField) : base(dataField) {
		Value = dataField.InitialValue;
		BooleanDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) { }

}



public class TextDataField : DataField {

	public TextDataFieldSpec TextDataFieldSpec { get; }

	public string Text {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Text));
		}
	}

	public TextDataField(TextDataFieldSpec dataField) : base(dataField) {
		Text = dataField.InitialValue;
		TextDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) {

		if (Text == string.Empty &&
		    (TextDataFieldSpec.MustNotBeEmpty || 
		     TextDataFieldSpec is { MustNotBeInitialValue: true, InitialValue: "" })) {

			addErrorString($"The data field \"{TextDataFieldSpec.Name}\" is empty.");
		}

		if (TextDataFieldSpec.MustNotBeInitialValue && Text == TextDataFieldSpec.InitialValue) {

			addErrorString($"The data field \"{TextDataFieldSpec.Name}\" must not be the default value {TextDataFieldSpec.InitialValue}.");
		}
	}

}



public class IntegerDataField : DataField {

	public IntegerDataFieldSpec IntegerDataFieldSpec { get; }

	public int MinValue => IntegerDataFieldSpec.MinValue;

	public int MaxValue => IntegerDataFieldSpec.MaxValue;

	public int Value {
		get;
		set {
			if (value > MaxValue) {
				field = MaxValue;
			} else if (value < MinValue) {
				field = MinValue;
			} else {
				field = value;
			}

			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
		}
	}

	public IntegerDataField(IntegerDataFieldSpec dataField) : base(dataField) {
		Value = dataField.InitialValue;
		IntegerDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) { }

}



public class SelectionDataField : DataField {

	public SelectionDataFieldSpec SelectionDataFieldSpec { get; }

	public ReadOnlyList<string> Options => SelectionDataFieldSpec.OptionNames;

	public Optional<string> SelectedOption {
		get;
		set {
			if (value != Optional.NoValue && !Options.Contains(value.Value)) {
				throw new InvalidOperationException();
			}

			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(SelectedOption));
		}
	} = Optional.NoValue;

	public SelectionDataField(SelectionDataFieldSpec dataField) : base(dataField) {
		SelectionDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) {

		if (SelectedOption == Optional<string>.NoValue) {
			addErrorString($"The data field \"{Name}\" does not have a selected value.");
		}
	}

}