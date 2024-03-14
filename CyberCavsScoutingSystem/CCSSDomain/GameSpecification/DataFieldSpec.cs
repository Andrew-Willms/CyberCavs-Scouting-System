using CCSSDomain.DataCollectors;
using ExhaustiveMatching;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



[Closed(typeof(BooleanDataFieldSpec), typeof(TextDataFieldSpec), typeof(IntegerDataFieldSpec), typeof(SelectionDataFieldSpec))]
public abstract class DataFieldSpec {

	public required string Name { get; init; }

	public abstract DataField ToDataField();

	public enum DataFieldType {
		Boolean,
		Text,
		Integer,
		Selection
	}

}

public class BooleanDataFieldSpec : DataFieldSpec {

	public required bool InitialValue { get; init; }

	public override DataField ToDataField() {
		return new BooleanDataField(this);
	}

}

public class TextDataFieldSpec : DataFieldSpec {

	public required string InitialValue { get; init; } = string.Empty;

	public required bool MustNotBeEmpty { get; init; }

	public required bool MustNotBeInitialValue { get; init; }

	public override DataField ToDataField() {
		return new TextDataField(this);
	}

}

public class IntegerDataFieldSpec : DataFieldSpec {

	public required int InitialValue { get; init; }

	public required int MinValue { get; init; } = int.MinValue;

	public required int MaxValue { get; init; } = int.MaxValue;

	public override DataField ToDataField() {
		return new IntegerDataField(this);
	}

}

public class SelectionDataFieldSpec : DataFieldSpec {

	public required ReadOnlyList<string> OptionNames { get; init; }

	public override DataField ToDataField() {
		return new SelectionDataField(this);
	}

}