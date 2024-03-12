using UtilitiesLibrary.Collections;
using static CCSSDomain.GameSpecification.DataFieldSpec;

namespace GameMakerWpf.Domain.EditingData;



public abstract class DataFieldEditingData {

	public required string Name { get; init; }

	public required DataFieldType DataFieldType { get; init; }

}

public class TextDataFieldEditingData : DataFieldEditingData { }

public class SelectionDataFieldEditingData : DataFieldEditingData {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldEditingData : DataFieldEditingData {

	public required string InitialValue { get; init; }
	public required string MinValue { get; init; }
	public required string MaxValue { get; init; }

}