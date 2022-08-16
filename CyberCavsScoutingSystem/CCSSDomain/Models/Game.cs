﻿using System;
using System.Collections.Generic;

namespace CCSSDomain.Models;



// Make this class immutable and have all the properties be generated by the GameEditingData on construction?
// random comment somewhere
public class Game {

	public Version Version;

	public DateTime VersionReleaseDate;

	public string Name = "";
	public string Description = "";
	public int Year = 0;

	public List<Alliance> Alliances = new();
}