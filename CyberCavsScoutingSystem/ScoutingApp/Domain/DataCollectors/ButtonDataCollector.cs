using System.ComponentModel;

//namespace ScoutingApp.Domain.DataCollectors;



//public class ButtonDataCollector : INotifyPropertyChanged {

//	private readonly IntegerDataField DataField;

//	public ButtonDataCollector(IntegerDataField dataField) {

//		DataField = dataField;

//		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
//	}

//	public void Increment() {

//		DataField.Value += 1;
//	}

//	public void Decrement() {

//		DataField.Value -= 1;
//	}

//	public int GetCurrentValue() {

//		return DataField.Value;
//	}

//	public event PropertyChangedEventHandler? PropertyChanged;

//	protected virtual void OnPropertyChanged(string propertyName) {
//		PropertyChanged?.Invoke(this, new(propertyName));
//	}

//}