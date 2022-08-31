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
			Year = DateTime.Now.Year
		};

		GameEditingData gameEditingData = new(initialValues);

		gameEditingData.Alliances.Add(new(gameEditingData, DefaultRedAlliance));
		gameEditingData.Alliances.Add(new(gameEditingData, DefaultBlueAlliance));

		return gameEditingData;
	}

	public static AllianceEditingData GetNewAlliance(GameEditingData gameEditingData) {

		HashSet<string> currentAllianceNames = gameEditingData.Alliances.SelectIfHasValue(x => x.Name.OutputObject).ToHashSet();

		int newAllianceNumber = 0;
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