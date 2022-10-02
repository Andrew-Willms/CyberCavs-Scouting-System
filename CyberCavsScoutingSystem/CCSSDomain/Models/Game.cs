using System;
using System.Collections.ObjectModel;

namespace CCSSDomain.Models;



public class Game {

	public Version Version { get; init; } = new(1, 0, 0);

	public DateTime VersionReleaseDate { get; } = DateTime.Now;

	public required string Name { get; init; }
	public string Description { get; init; } = "";
	public required int Year { get; init; }

	public required uint RobotsPerAlliance { get; init; }
	public required uint AlliancesPerMatch { get; init; }

	public required ReadOnlyCollection<Alliance> Alliances { get; init; }

	public required ReadOnlyCollection<DataField> DataFields { get; init; }
}