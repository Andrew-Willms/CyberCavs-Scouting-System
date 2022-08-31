using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CCSSDomain.Models;



public class Game {

	public Version Version { get; init; } = new(1, 0, 0);

	public DateTime VersionReleaseDate { get; init; } = DateTime.Now;

	public string Name { get; init; } = "";
	public string Description { get; init; } = "";
	public int Year { get; init; } = 0;

	public uint RobotsPerAlliance { get; init; } = 0;
	public uint AlliancesPerMatch { get; init; } = 0;

	public ReadOnlyCollection<Alliance> Alliances { get; init; } = new List<Alliance>().AsReadOnly();
}