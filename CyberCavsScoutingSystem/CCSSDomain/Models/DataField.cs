using System.Collections.ObjectModel;

namespace CCSSDomain.Models;



public enum DataFieldType {
	Text,
	Dropdown,
	Counter
}

public abstract class DataField {

	public required DataFieldType Type { get; init; }

	public required string Name { get; init; }
}

public class TextDataField : DataField { }

public class DropdownDataField : DataField {

	public required ReadOnlyCollection<string> Options { get; init; }
}

public class CounterDataField : DataField {

	public int InitialValue { get; init; }
}