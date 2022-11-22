using System.Collections.ObjectModel;

namespace CCSSDomain.Models;



public abstract class DataField {

	public required string Name { get; init; }

	public enum DataFieldType {
		Text,
		Selection,
		Integer
	}

}

public class TextDataField : DataField { }

public class SelectionDataField : DataField {

	public required ReadOnlyCollection<string> OptionNames { get; init; }

}

public class IntegerDataField : DataField {

	public int InitialValue { get; init; } = 0;

	public int MinValue { get; init; } = int.MinValue;

	public int MaxValue { get; init; } = int.MaxValue;

}