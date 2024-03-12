using OneOf;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public abstract class DataFieldSpecBase {

	public required string Name { get; init; }

}

[GenerateOneOf]
public partial class
	DataFieldSpec : OneOfBase<BooleanDataFieldSpec, TextDataFieldSpec, IntegerDataFieldSpec, SelectionDataFieldSpec> {

	public DataFieldSpecBase AsBase => Match<DataFieldSpecBase>(
		booleanDataFieldSpec => booleanDataFieldSpec,
		textDataFieldSpec => textDataFieldSpec,
		integerDataFieldSpec => integerDataFieldSpec,
		selectionDataFieldSpec => selectionDataFieldSpec);

	public enum DataFieldType {
		Boolean,
		Text,
		Integer,
		Selection
	}

}

public class BooleanDataFieldSpec : DataFieldSpecBase {

	public bool InitialValue { get; init; } = false;

}

public class TextDataFieldSpec : DataFieldSpecBase {

	public string InitialValue { get; init; } = string.Empty;

	public bool MustNotBeEmpty { get; init; }

	public bool MustNotBeInitialValue { get; init; }

}

public class SelectionDataFieldSpec : DataFieldSpecBase {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldSpec : DataFieldSpecBase {

	public int InitialValue { get; init; }

	public int MinValue { get; init; } = int.MinValue;

	public int MaxValue { get; init; } = int.MaxValue;

}