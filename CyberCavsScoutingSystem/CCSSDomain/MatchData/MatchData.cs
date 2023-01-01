using System.ComponentModel;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.SimpleEvent;

namespace CCSSDomain.MatchData; 



public class MatchData {

	public MatchData(GameSpecification.Game gameSpecification) {



	}



}



public abstract class DataField {

	public required string Name { get; init; }

	public readonly Event OnValueChange = new();

}

public class TextDataField : DataField {

	public string Text { get; set; }

}

public class IntegerDataField : DataField {

	public required int MinValue { get; init; }

	public required int MaxValue { get; init; }

	private int _Value;
	public int Value {
		get => _Value;
		set {
			_Value = value;
			OnValueChange.Invoke();
		}
	}

}

public class SelectionDataField : DataField {

	public required string[] Options { get; init; }

	public Optional<string> SelectedOption { get; set; } = Optional.NoValue;

}



public class Input : INotifyPropertyChanged {

	private readonly DataField DataField;

	public Input(DataField dataField) {

		DataField = dataField;
	}

	public void Update() { }

	public void OnValueChange() {

	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public class Counter : INotifyPropertyChanged {

	private readonly IntegerDataField DataField;

	public Counter(IntegerDataField dataField) {

		DataField = dataField;

		DataField.OnValueChange.Subscribe(() => OnPropertyChanged(""));
	}

	public void Increment() {

		DataField.Value += 1;
	}

	public void Decrement() {

		DataField.Value -= 1;
	}

	public int GetCurrentValue() {

		return DataField.Value;
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}