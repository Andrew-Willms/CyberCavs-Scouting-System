using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.Domain.EditingData; 



public class GameEditingData {

	public required string Name { get; init; }
	public required string Year { get; init; }
	public required string Description { get; init; }

	public required string VersionMajorNumber { get; init; }
	public required string VersionMinorNumber { get; init; }
	public required string VersionPatchNumber { get; init; }
	public required string VersionDescription { get; init; }

	public required string RobotsPerAlliance { get; init; }
	public required string AlliancesPerMatch { get; init; }

	public required IEnumerable<AllianceEditingData> Alliances { get; init; }
	public required IEnumerable<DataFieldEditingData> DataFields { get; init; }

	public GameEditingData() { }

	[JsonConstructor]
	[SetsRequiredMembers]
	public GameEditingData(
		string name, string year, string description,
		string versionMajorNumber, string versionMinorNumber, string versionPatchNumber, string versionDescription,
		string robotsPerAlliance, string alliancesPerMatch,
		IEnumerable<AllianceEditingData> alliances, IEnumerable<DataFieldEditingData> dataFields) {

		Name = name;
		Year = year;
		Description = description;

		VersionMajorNumber = versionMajorNumber;
		VersionMinorNumber = versionMinorNumber;
		VersionPatchNumber = versionPatchNumber;
		VersionDescription = versionDescription;

		RobotsPerAlliance = robotsPerAlliance;
		AlliancesPerMatch = alliancesPerMatch;

		Alliances = alliances.ToReadOnly();
		DataFields = dataFields.ToReadOnly();
	}

}