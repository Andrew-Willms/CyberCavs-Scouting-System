using System.ComponentModel;

namespace CCSSDomain.DataCollectors;



public class ButtonDataCollector : INotifyPropertyChanged {

	private readonly IntegerDataField DataField;

	public ButtonDataCollector(IntegerDataField dataField) {

		DataField = dataField;
		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public void Increment() {
		DataField.Value += 1;
	}

	public void Decrement() {
		DataField.Value -= 1;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}