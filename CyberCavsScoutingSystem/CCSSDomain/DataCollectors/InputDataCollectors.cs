using System.ComponentModel;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace CCSSDomain.DataCollectors;



public abstract class InputDataCollector : INotifyPropertyChanged {

	public required string Label { get; init; }

	public static InputDataCollector FromDataField(InputSpec inputSpec, DataField dataField) {

		return dataField.Match<InputDataCollector>(
			booleanDataField => new BooleanInputDataCollector(booleanDataField) { Label = inputSpec.Label }, 
			textDataField => new TextInputDataCollector(textDataField) { Label = inputSpec.Label },
			integerDataField => new IntegerInputDataCollector(integerDataField) { Label = inputSpec.Label },
			selectionDataField => new SelectionInputDataCollector(selectionDataField) { Label = inputSpec.Label }
		);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public class BooleanInputDataCollector : InputDataCollector, INotifyPropertyChanged {

	private readonly BooleanDataField DataField;

	public BooleanInputDataCollector(BooleanDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public bool Value {
		get => DataField.Value;
		set => DataField.Value = value;
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

	public ReadOnlyList<string> Options => DataField.Options;

	public SelectionInputDataCollector(SelectionDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(nameof(SelectedOption)));
	}

	public string SelectedOption {
		get => DataField.SelectedOption.HasValue ? DataField.SelectedOption.Value : "";
		set => DataField.SelectedOption = value == "" ? Optional.NoValue : value.Optionalize();
	}

}