using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using static CCSSDomain.Models.DataField;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.Domain.EditingData; 



[JsonDerivedType(typeof(TextDataFieldEditingData), typeDiscriminator: "Text")]
[JsonDerivedType(typeof(SelectionDataFieldEditingData), typeDiscriminator: "Selection")]
[JsonDerivedType(typeof(IntegerDataFieldEditingData), typeDiscriminator: "Integer")]
public abstract class DataFieldEditingData {

	public required string Name { get; init; }

	public required DataFieldType DataFieldType { get; set; }

	public DataFieldEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public DataFieldEditingData(string name, DataFieldType dataFieldType) {
		Name = name;
		DataFieldType = dataFieldType;
	}

}

public class TextDataFieldEditingData : DataFieldEditingData {

	public TextDataFieldEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public TextDataFieldEditingData(string name, DataFieldType dataFieldType) : base(name, dataFieldType) { }

}

public class SelectionDataFieldEditingData : DataFieldEditingData {

	public required IEnumerable<string> OptionNames { get; init; }

	public SelectionDataFieldEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public SelectionDataFieldEditingData(string name, DataFieldType dataFieldType, IEnumerable<string> optionNames) 
		:base(name, dataFieldType) {

		OptionNames = optionNames.ToReadOnly();
	}

}

public class IntegerDataFieldEditingData : DataFieldEditingData {

	public required string InitialValue { get; init; }
	public required string MinValue { get; init; }
	public required string MaxValue { get; init; }

	public IntegerDataFieldEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public IntegerDataFieldEditingData(string name, DataFieldType dataFieldType,
		string initialValue, string minValue, string maxValue): base(name, dataFieldType) {

		InitialValue = initialValue;
		MinValue = minValue;
		MaxValue = maxValue;
	}

}