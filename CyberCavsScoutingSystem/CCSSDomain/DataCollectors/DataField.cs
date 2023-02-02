using System.Diagnostics;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace CCSSDomain.DataCollectors;



public abstract class DataField {

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

            } else if (value < MaxValue) {
                _Value = MaxValue;
            }

            _Value = value;
            OnValueChange.Invoke();
        }
    }

    public IntegerDataField(IntegerDataFieldSpec dataField) : base(dataField) {
        IntegerDataFieldSpec = dataField;
    }

}

public class SelectionDataField : DataField {

    public SelectionDataFieldSpec SelectionDataFieldSpec { get; }

    public ReadOnlyList<string> Options => SelectionDataFieldSpec.OptionNames;

    private string _SelectedOption = "";
    public string SelectedOption {
        get => _SelectedOption;
        set {
            _SelectedOption = Options.Contains(value) ? value : "";
            OnValueChange.Invoke();
        }
    }

    public SelectionDataField(SelectionDataFieldSpec dataField) : base(dataField) {
        SelectionDataFieldSpec = dataField;
    }

}