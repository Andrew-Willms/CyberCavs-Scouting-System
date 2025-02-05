using UtilitiesLibrary.Collections;

namespace GameMakerWpf.Domain.EditingData;



public abstract class DataFieldEditingData {

	public required string Name { get; init; }

}



public class BooleanDataFieldEditingData : DataFieldEditingData {

	public required bool InitialValue { get; init; }

}



public class TextDataFieldEditingData : DataFieldEditingData {

	public required string InitialValue { get; init; }

	public required bool MustNotBeEmpty { get; init; }

	public required bool MustNotBeInitialValue { get; init; }

}



public class SelectionDataFieldEditingData : DataFieldEditingData {

	public required ReadOnlyList<string> OptionNames { get; init; }

	public required bool RequiresValue { get; init; }

}



public class IntegerDataFieldEditingData : DataFieldEditingData {

	public required string InitialValue { get; init; }

	public required string MinValue { get; init; }

	public required string MaxValue { get; init; }

}