using System;
using System.Collections.Generic;
using CCSSDomain.Models;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.Domain.Data;



public static class DefaultEditingDataValues {

	private static readonly AllianceEditingData DefaultRedAlliance = new() {
		Name = "Red Alliance",
		RedColorValue = $"{255}",
		GreenColorValue = $"{0}",
		BlueColorValue = $"{0}"
	};

	private static readonly AllianceEditingData DefaultBlueAlliance = new() {
		Name = "Blue Alliance",
		RedColorValue = $"{0}",
		GreenColorValue = $"{0}",
		BlueColorValue = $"{255}",
	};



	public static readonly TextDataFieldEditingData DefaultTextDataFieldEditingData = new() {
		Name = "New Data Field",
		DataFieldType = DataField.DataFieldType.Text
	};

	public static readonly SelectionDataFieldEditingData DefaultSelectionDataFieldEditingData = new() {
		Name = "New Data Field",
		OptionNames = ReadOnlyList.Empty,
		DataFieldType = DataField.DataFieldType.Selection
	};

	public static readonly IntegerDataFieldEditingData DefaultIntegerDataFieldEditingData = new() {
		Name = "New Data Field",
		InitialValue = $"{0}",
		MinValue = $"{int.MinValue}",
		MaxValue = $"{int.MaxValue}",
		DataFieldType = DataField.DataFieldType.Integer
	};

	public static readonly DataFieldEditingData DefaultDataFieldEditingData = DefaultTextDataFieldEditingData;



	public static GameEditingData DefaultGameEditingData => new() {

		Name = GameNameGenerator.GetRandomGameName(),
		Year = DateTime.Now.Year.ToString(),
		Description = "",

		VersionMajorNumber = $"{1}",
		VersionMinorNumber = $"{0}",
		VersionPatchNumber = $"{0}",
		VersionDescription = "",

		RobotsPerAlliance = $"{3}",
		AlliancesPerMatch = $"{2}",
		Alliances = new List<AllianceEditingData> { DefaultRedAlliance, DefaultBlueAlliance }.ToReadOnly(),
		DataFields = ReadOnlyList.Empty,
	};



	public static void AddUniqueAlliance(this GameEditor gameEditor) {

		gameEditor.AddAlliance(AllianceGenerator.GenerateUniqueAlliance(gameEditor.Alliances.SelectIfHasValue(x => x.Name.OutputObject)));
	}

}