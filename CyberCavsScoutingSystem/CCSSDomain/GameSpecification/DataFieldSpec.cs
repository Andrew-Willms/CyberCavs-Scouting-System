using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public abstract class DataFieldSpec {

	public required string Name { get; init; }

	public enum DataFieldType {
		Text,
		Selection,
		Integer
	}

}

public class TextDataFieldSpec : DataFieldSpec { }

public class SelectionDataFieldSpec : DataFieldSpec {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldSpec : DataFieldSpec {

	public int InitialValue { get; init; } = 0;

	public int MinValue { get; init; } = int.MinValue;

	public int MaxValue { get; init; } = int.MaxValue;

}