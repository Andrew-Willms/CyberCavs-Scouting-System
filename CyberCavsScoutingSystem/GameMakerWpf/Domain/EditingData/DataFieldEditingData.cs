using OneOf;
using UtilitiesLibrary.Collections;
using static CCSSDomain.GameSpecification.DataFieldSpec;

namespace GameMakerWpf.Domain.EditingData;



public abstract class DataFieldEditingDataBase {

	public required string Name { get; init; }

	public required DataFieldType DataFieldType { get; init; }

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



public class BooleanDataFieldEditingData : DataFieldEditingDataBase;

public class TextDataFieldEditingData : DataFieldEditingDataBase;

public class SelectionDataFieldEditingData : DataFieldEditingDataBase {

	public required ReadOnlyList<string> OptionNames { get; init; }

}

public class IntegerDataFieldEditingData : DataFieldEditingDataBase {

	public required string InitialValue { get; init; }
	public required string MinValue { get; init; }
	public required string MaxValue { get; init; }

}