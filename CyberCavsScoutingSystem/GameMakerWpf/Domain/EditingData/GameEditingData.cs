using UtilitiesLibrary.Collections;

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

	public required ReadOnlyList<AllianceEditingData> Alliances { get; init; }
	public required ReadOnlyList<DataFieldEditingData> DataFields { get; init; }

	public required ReadOnlyList<InputEditingData> SetupTabInputs { get; init; }
	public required ReadOnlyList<InputEditingData> AutoTabInputs { get; init; }
	public required ReadOnlyList<InputEditingData> TeleTabInputs { get; init; }
	public required ReadOnlyList<InputEditingData> EndgameTabInputs { get; init; }

	public required ReadOnlyList<ButtonEditingData> AutoButtons { get; init; }
	public required ReadOnlyList<ButtonEditingData> TeleButtons { get; init; }

}