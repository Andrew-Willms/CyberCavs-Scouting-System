using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CCSSDomain.Models;



public class Game {

	public Version Version { get; init; } = new(1, 0, 0);

	public DateTime VersionReleaseDate { get; } = DateTime.Now;

	public string Name { get; init; } = "";
	public string Description { get; init; } = "";
	public int Year { get; init; }

	public uint RobotsPerAlliance { get; init; }
	public uint AlliancesPerMatch { get; init; }

	public ReadOnlyCollection<Alliance> Alliances { get; init; } = new List<Alliance>().AsReadOnly();

	public ReadOnlyCollection<DataField> DataFields { get; init; } = new List<DataField>().AsReadOnly();
}