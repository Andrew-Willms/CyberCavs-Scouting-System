using CCSSDomain.DataCollectors;
using OneOf;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public abstract class DataFieldSpecBase {

	public required string Name { get; init; }

	public abstract DataField ToDataField();

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

	public required bool InitialValue { get; init; }

	public override DataField ToDataField() {
		return new BooleanDataField(this);
	}

}

public class TextDataFieldSpec : DataFieldSpecBase {

	public required string InitialValue { get; init; } = string.Empty;

	public required bool MustNotBeEmpty { get; init; }

	public required bool MustNotBeInitialValue { get; init; }

	public override DataField ToDataField() {
		return new TextDataField(this);
	}

}

public class IntegerDataFieldSpec : DataFieldSpecBase {

	public required int InitialValue { get; init; }

	public required int MinValue { get; init; } = int.MinValue;

	public required int MaxValue { get; init; } = int.MaxValue;

	public override DataField ToDataField() {
		return new IntegerDataField(this);
	}

}

public class SelectionDataFieldSpec : DataFieldSpecBase {

	public required ReadOnlyList<string> OptionNames { get; init; }

	public override DataField ToDataField() {
		return new SelectionDataField(this);
	}

}