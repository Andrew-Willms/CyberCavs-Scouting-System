using System;
using System.Windows.Media;
using CCSSDomain.Models;
using GameMakerWpf.Domain;
using UtilitiesLibrary;
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

	public static GameEditingData DefaultEditingData {

		get {
			Game initialValues = new() {
				Name = GameNameGenerator.GetRandomGameName(),
				Year = DateTime.Now.Year,
				RobotsPerAlliance = 3,
				AlliancesPerMatch = 2,
				Alliances = new ReadOnlyList<Alliance>(),
				DataFields = new ReadOnlyList<DataField>()
			};

			GameEditingData gameEditingData = new(initialValues);

			gameEditingData.AddAlliance(new(gameEditingData, DefaultRedAlliance));
			gameEditingData.AddAlliance(new(gameEditingData, DefaultBlueAlliance));

			return gameEditingData;
		}
	}

	public static AllianceEditingData GetNewAlliance(GameEditingData gameEditingData) {

		Alliance initialValues = AllianceGenerator.GenerateAlliance(gameEditingData.Alliances.SelectIfHasValue(x => x.Name.OutputObject));
		return new(gameEditingData, initialValues);
	}

	public static DataFieldEditingData GetNewDataField(GameEditingData gameEditingData) {

		TextDataField initialValues = new() { Name = "New Data Field" };
		return new TextDataFieldEditingData(gameEditingData, initialValues);
	}

}