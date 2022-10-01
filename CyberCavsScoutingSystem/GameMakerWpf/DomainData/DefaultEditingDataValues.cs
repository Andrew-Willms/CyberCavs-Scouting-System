using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using CCSSDomain.Models;
using GameMakerWpf.Domain;
using UtilitiesLibrary.Extensions;

namespace GameMakerWpf.DomainData; 



public static class DefaultEditingDataValues {

	private static readonly Alliance DefaultRedAlliance = new() {
		Name = "Red Alliance",
		Color = Colors.Red
	};

	private static readonly Alliance DefaultBlueAlliance = new() {
		Name = "Blue Alliance",
		Color = Colors.Blue
	};

	public static GameEditingData GetDefaultEditingData() {

		Game initialValues = new() {
			Name = GameNameGenerator.GetRandomGameName(),
			Year = DateTime.Now.Year,
			RobotsPerAlliance = 3,
			AlliancesPerMatch = 2
		};

		GameEditingData gameEditingData = new(initialValues);

		gameEditingData.Alliances.Add(new(gameEditingData, DefaultRedAlliance));
		gameEditingData.Alliances.Add(new(gameEditingData, DefaultBlueAlliance));

		return gameEditingData;
	}

	public static AllianceEditingData GetNewAlliance(IEnumerable<string> allianceNames) {

		//System.Windows.Media.Colors.

		HashSet<string> currentAllianceNames = allianceNames.ToHashSet();

		int newAllianceNumber = 1;
		while (currentAllianceNames.Contains($"Alliance {newAllianceNumber}")) {
			newAllianceNumber++;
		}

		Random random = new();
		byte[] colorValues = new byte[3];
		random.NextBytes(colorValues);

		Alliance initialValues = new() {
			Name = $"Alliance {newAllianceNumber}",
			Color = Color.FromRgb(colorValues[0], colorValues[1], colorValues[2])
		};

		return new(gameEditingData, initialValues);
	}

}