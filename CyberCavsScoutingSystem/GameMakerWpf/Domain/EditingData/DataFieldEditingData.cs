using System;
using OneOf;
using UtilitiesLibrary.Collections;
using static CCSSDomain.GameSpecification.DataFieldSpec;

namespace GameMakerWpf.Domain.EditingData;



public abstract class DataFieldEditingDataBase {

	public required string Name { get; init; }

	public required DataFieldType DataFieldType { get; init; }

	// yes this is a hack I know, I probably shouldn't be trying to use unions for everything
	public DataFieldEditingData ToOneOf() {

		return this switch {
			BooleanDataFieldEditingData booleanDataFieldEditingData => booleanDataFieldEditingData,
			IntegerDataFieldEditingData integerDataFieldEditingData => integerDataFieldEditingData,
			SelectionDataFieldEditingData selectionDataFieldEditingData => selectionDataFieldEditingData,
			TextDataFieldEditingData textDataFieldEditingData => textDataFieldEditingData,
			_ => throw new ArgumentOutOfRangeException()
		};

	}

}


[GenerateOneOf]
public partial class DataFieldEditingData : 
	OneOfBase<BooleanDataFieldEditingData, TextDataFieldEditingData, IntegerDataFieldEditingData, SelectionDataFieldEditingData> {

	public DataFieldEditingDataBase AsBase => Match<DataFieldEditingDataBase>(
		booleanDataFieldEditingData => booleanDataFieldEditingData,
		textDataFieldEditingData => textDataFieldEditingData,
		integerDataFieldEditingData => integerDataFieldEditingData,
		selectionDataFieldEditingData => selectionDataFieldEditingData 
	);

}



public class BooleanDataFieldEditingData : DataFieldEditingDataBase {

	public required bool InitialValue { get; init; }

}

public class TextDataFieldEditingData : DataFieldEditingDataBase {

	public required string InitialValue { get; init; }
	public required bool MustNotBeEmpty { get; init; }
	public required bool MustNotBeInitialValue { get; init; }

}

public class SelectionDataFieldEditingData : DataFieldEditingDataBase {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldEditingData : DataFieldEditingDataBase {

	public required string InitialValue { get; init; }
	public required string MinValue { get; init; }
	public required string MaxValue { get; init; }

}