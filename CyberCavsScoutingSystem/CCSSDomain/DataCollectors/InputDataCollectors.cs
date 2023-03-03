using System.ComponentModel;
using System.Diagnostics;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.DataCollectors;



public abstract class InputDataCollector : INotifyPropertyChanged {

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

    public static InputDataCollector FromDataField(DataField inputSpec) {

        return inputSpec switch {
			TextDataField textDataField => new TextInputDataCollector(textDataField),
			IntegerDataField integerDataField => new IntegerInputDataCollector(integerDataField),
			SelectionDataField selectionDataField => new SelectionInputDataCollector(selectionDataField),
			_ => throw new UnreachableException()
        };
    }

}



public class TextInputDataCollector : InputDataCollector, INotifyPropertyChanged {

	private readonly TextDataField DataField;

	public TextInputDataCollector(TextDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public string Text {
		get => DataField.Text;
		set => DataField.Text = value;
	}

}



public class IntegerInputDataCollector : InputDataCollector, INotifyPropertyChanged {

	private readonly IntegerDataField DataField;

	public IntegerInputDataCollector(IntegerDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public int Value {
		get => DataField.Value;
		set => DataField.Value = value;
	}

}



public class SelectionInputDataCollector : InputDataCollector, INotifyPropertyChanged {

	private readonly SelectionDataField DataField;

	private ReadOnlyList<string> Options => DataField.Options;

	public SelectionInputDataCollector(SelectionDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public string Value {
		get => DataField.SelectedOption;
		set => DataField.SelectedOption = value;
	}

}