using System;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public class Game {

	public Version Version { get; init; } = new(1, 0, 0);

	public DateTime VersionReleaseDate { get; } = DateTime.Now;

	public required string Name { get; init; }
	public string Description { get; init; } = "";
	public required int Year { get; init; }

	public required uint RobotsPerAlliance { get; init; }
	public required uint AlliancesPerMatch { get; init; }

	public required ReadOnlyList<Alliance> Alliances { get; init; }

	public required ReadOnlyList<DataField> DataFields { get; init; }

	public required ReadOnlyList<Input> SetupTabInputs { get; init; }
	public required ReadOnlyList<Input> AutoTabInputs { get; init; }
	public required ReadOnlyList<Input> TeleTabInputs { get; init; }
	public required ReadOnlyList<Input> EndgameTabInputs { get; init; }

	public required ReadOnlyList<Button> AutoButtons { get; init; }
	public required ReadOnlyList<Button> TeleButtons { get; init; }

}