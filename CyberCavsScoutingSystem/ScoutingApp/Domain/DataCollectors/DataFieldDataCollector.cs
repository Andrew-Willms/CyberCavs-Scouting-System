//using System;
//using UtilitiesLibrary.Optional;
//using UtilitiesLibrary.SimpleEvent;

//namespace ScoutingApp.Domain.DataCollectors;



//public abstract class DataField {

//	public required string Name { get; init; }

//	public readonly Event OnValueChange = new();

//}

//public class TextDataField : DataField {

//	public string Text { get; set; }

//}

//public class IntegerDataField : DataField {

//	public required int MinValue { get; init; }

//	public required int MaxValue { get; init; }

//	private int _Value;
//	public int Value {
//		get => _Value;
//		set {
//			_Value = value;
//			OnValueChange.Invoke();
//		}
//	}

//}

//public class SelectionDataField : DataField {

//	public required string[] Options { get; init; }

//	public Optional<string> SelectedOption { get; set; } = Optional.NoValue;

//}