using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace CCSSDomain.DataCollectors;



public abstract class DataField : INotifyPropertyChanged {

	public DataFieldSpec DataFieldSpec { get; }

	public string Name => DataFieldSpec.Name;

	public readonly Event OnValueChange = new();

	public abstract List<string> Errors { get; }

	protected DataField(DataFieldSpec dataFieldSpec) {
		DataFieldSpec = dataFieldSpec;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

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

	public override List<string> Errors => [];

	public BooleanDataField(BooleanDataFieldSpec dataField) : base(dataField) {
		Value = dataField.InitialValue;
		BooleanDataFieldSpec = dataField;
	}

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

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (Text == string.Empty &&
			    (TextDataFieldSpec.MustNotBeEmpty ||
			     TextDataFieldSpec is { MustNotBeInitialValue: true, InitialValue: "" })) {

				errors.Add($"The data field \"{Name}\" is empty.");
			}

			if (TextDataFieldSpec.MustNotBeInitialValue && Text == TextDataFieldSpec.InitialValue) {

				errors.Add($"The data field \"{Name}\" must not be the default value {TextDataFieldSpec.InitialValue}.");
			}

			return errors;
		}
	}

	public TextDataField(TextDataFieldSpec dataField) : base(dataField) {
		Text = dataField.InitialValue;
		TextDataFieldSpec = dataField;
	}

}



public class IntegerDataField : DataField {

	public IntegerDataFieldSpec IntegerDataFieldSpec { get; }

	public int MinValue => IntegerDataFieldSpec.MinValue;

	public int MaxValue => IntegerDataFieldSpec.MaxValue;

	public int Value {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
		}
	}

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (Value > MaxValue) {
				errors.Add($"The data field \"{Name}\" ist set to {Value} which is greater than it's maximum value of {MaxValue}.");
			}

			if (Value < MinValue) {
				errors.Add($"The data field \"{Name}\" ist set to {Value} which is less than it's minimum value of {MinValue}.");
			}

			return errors;
		}
	}

	public IntegerDataField(IntegerDataFieldSpec dataField) : base(dataField) {
		Value = dataField.InitialValue;
		IntegerDataFieldSpec = dataField;
	}

}



public class SelectionDataField : DataField {

	public SelectionDataFieldSpec SelectionDataFieldSpec { get; }

	public ReadOnlyList<string> Options => SelectionDataFieldSpec.OptionNames;

	public Optional<string> SelectedOption {
		get;
		set {
			field = value;
			OnValueChange.Invoke();
			OnPropertyChanged(nameof(SelectedOption));
		}
	} = Optional.NoValue;

	public override List<string> Errors {
		get {
			List<string> errors = [];

			if (!SelectionDataFieldSpec.RequiresValue && SelectedOption == Optional<string>.NoValue) {
				errors.Add($"The data field \"{Name}\" requires a value value.");
			}

			if (SelectedOption != Optional.NoValue && !Options.Contains(SelectedOption.Value)) {
				errors.Add($"The data field \"{Name}\" does not contain the specified value '{SelectedOption.Value}'");
			}

			return errors;
		}
	}

	public SelectionDataField(SelectionDataFieldSpec dataField) : base(dataField) {
		SelectionDataFieldSpec = dataField;
	}

}