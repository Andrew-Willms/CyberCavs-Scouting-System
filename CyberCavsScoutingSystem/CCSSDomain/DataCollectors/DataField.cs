using System;
using System.ComponentModel;
using System.Linq;
using CCSSDomain.GameSpecification;
using OneOf;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace CCSSDomain.DataCollectors;



public abstract class DataFieldBase : INotifyPropertyChanged {

	public DataFieldSpec DataFieldSpec { get; }

	public string Name => DataFieldSpec.AsBase.Name;

	public readonly Event OnValueChange = new();

	protected DataFieldBase(DataFieldSpec dataFieldSpec) {
		DataFieldSpec = dataFieldSpec;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

	public abstract void AddErrorsToCollection(Action<string> addErrorString);

}


[GenerateOneOf]
public partial class DataField : OneOfBase<BooleanDataField, TextDataField, IntegerDataField, SelectionDataField> {

	public DataFieldBase AsBase => Match<DataFieldBase>(
		booleanDataField => booleanDataField,
		dataFieldSpec => dataFieldSpec,
		integerDataField => integerDataField,
		selectionDataField => selectionDataField);

	public static DataField FromSpec(DataFieldSpec dataFieldSpec) {

		return dataFieldSpec.Match<DataField>(
			booleanDataFieldSpec => new BooleanDataField(booleanDataFieldSpec),
			textDataFieldSpec => new TextDataField(textDataFieldSpec),
			integerDataFieldSpec => new IntegerDataField(integerDataFieldSpec),
			selectionDataFieldSpec => new SelectionDataField(selectionDataFieldSpec));
	}

}



public class BooleanDataField : DataFieldBase {

	public BooleanDataFieldSpec BooleanDataFieldSpec { get; }

	private bool _Value;
	public bool Value {
		get => _Value;
		set {
			_Value = value;
			OnValueChange.Invoke();
		}
	}

	public BooleanDataField(BooleanDataFieldSpec dataField) : base(dataField) {
		_Value = dataField.InitialValue;
		BooleanDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) { }

}



public class TextDataField : DataFieldBase {

	public TextDataFieldSpec TextDataFieldSpec { get; }

	private string _Text;
	public string Text {
		get => _Text;
		set {
			_Text = value;
			OnValueChange.Invoke();
		}
	}

	public TextDataField(TextDataFieldSpec dataField) : base(dataField) {
		_Text = dataField.InitialValue;
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



public class IntegerDataField : DataFieldBase {

	public IntegerDataFieldSpec IntegerDataFieldSpec { get; }

	public int MinValue => IntegerDataFieldSpec.MinValue;

	public int MaxValue => IntegerDataFieldSpec.MaxValue;

	private int _Value;
	public int Value {
		get => _Value;
		set {

			if (value > MaxValue) {
				_Value = MaxValue;

			} else if (value < MinValue) {
				_Value = MinValue;

			} else {
				_Value = value;
			}

			OnValueChange.Invoke();
			OnPropertyChanged(nameof(Value));
		}
	}

	public IntegerDataField(IntegerDataFieldSpec dataField) : base(dataField) {
		_Value = dataField.InitialValue;
		IntegerDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) { }

}



public class SelectionDataField : DataFieldBase {

	public SelectionDataFieldSpec SelectionDataFieldSpec { get; }

	public ReadOnlyList<string> Options => SelectionDataFieldSpec.OptionNames;

	private Optional<string> _SelectedOption = Optional.NoValue;
	public Optional<string> SelectedOption {
		get => _SelectedOption;
		set {

			if (value != Optional.NoValue && !Options.Contains(value.Value)) {
				throw new InvalidOperationException();
			}

			_SelectedOption = value;
			OnValueChange.Invoke();
		}
	}

	public SelectionDataField(SelectionDataFieldSpec dataField) : base(dataField) {
		SelectionDataFieldSpec = dataField;
	}

	public override void AddErrorsToCollection(Action<string> addErrorString) {

		if (SelectedOption == Optional<string>.NoValue) {
			addErrorString($"The data field \"{Name}\" does not have a selected value.");
		}
	}

}