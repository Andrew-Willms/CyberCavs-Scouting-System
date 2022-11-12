using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GameMakerWpf.Domain.EditingData; 



public class AllianceEditingData {

	public required string Name { get; init; }

	public required string RedColorValue { get; init; }
	public required string GreenColorValue { get; init; }
	public required string BlueColorValue { get; init; }

	public AllianceEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public AllianceEditingData(string name, string redColorValue, string greenColorValue, string blueColorValue) {
		Name = name;
		RedColorValue = redColorValue;
		GreenColorValue = greenColorValue;
		BlueColorValue = blueColorValue;
	}

}