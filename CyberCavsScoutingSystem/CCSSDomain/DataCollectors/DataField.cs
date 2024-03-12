using System;
using System.ComponentModel;
using System.Diagnostics;
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

    protected DataField(DataFieldSpec dataFieldSpec) {
        DataFieldSpec = dataFieldSpec;
    }

    public static DataField FromSpec(DataFieldSpec dataFieldSpec) {

        return dataFieldSpec switch {
            TextDataFieldSpec textDataFieldSpec => new TextDataField(textDataFieldSpec),
            IntegerDataFieldSpec integerDataFieldSpec => new IntegerDataField(integerDataFieldSpec),
            SelectionDataFieldSpec selectionDataFieldSpec => new SelectionDataField(selectionDataFieldSpec),
            _ => throw new UnreachableException()
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) {
	    PropertyChanged?.Invoke(this, new(propertyName));
    }

}

public class TextDataField : DataField {

    public TextDataFieldSpec TextDataFieldSpec { get; }

    private string _Text = "";
    public string Text {
        get => _Text;
        set {
            _Text = value;
            OnValueChange.Invoke();
        }
    }

    public TextDataField(TextDataFieldSpec dataField) : base(dataField) {
        TextDataFieldSpec = dataField;
    }

}

public class IntegerDataField : DataField {

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
        IntegerDataFieldSpec = dataField;
    }

}

public class SelectionDataField : DataField {

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

}