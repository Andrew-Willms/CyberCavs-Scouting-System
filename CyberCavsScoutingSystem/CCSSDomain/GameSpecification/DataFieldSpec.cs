using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public abstract class DataFieldSpec {

	public required string Name { get; init; }

	public enum DataFieldType {
		Boolean,
		Text,
		Integer,
		Selection
	}

}

public class BooleanDataFieldSpec : DataFieldSpec {

	public bool InitialValue { get; init; } = false;

}

public class TextDataFieldSpec : DataFieldSpec {

	public string InitialValue { get; init; } = string.Empty;

}

public class SelectionDataFieldSpec : DataFieldSpec {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldSpec : DataFieldSpec {

	public int InitialValue { get; init; }

	public int MinValue { get; init; } = int.MinValue;

	public int MaxValue { get; init; } = int.MaxValue;

}