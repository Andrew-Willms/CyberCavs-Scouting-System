//using System.ComponentModel;

//namespace ScoutingApp.Domain.DataCollectors;



//public class InputDataCollector : INotifyPropertyChanged {

//	private readonly DataFieldDataCollector _DataField;

//	public InputDataCollector(DataFieldDataCollector dataFieldDataCollector) {

//		_DataField = dataFieldDataCollector;
//	}

//	public void Update() { }

//	public void OnValueChange() {

//	}

//	public event PropertyChangedEventHandler? PropertyChanged;

//	protected virtual void OnPropertyChanged(string propertyName) {
//		PropertyChanged?.Invoke(this, new(propertyName));
//	}

//}