using System;
using System.Collections.Generic;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Collections;

namespace GameMakerWpf.Domain.Data;



public static class DefaultEditingDataValues {

	private static readonly AllianceEditingData DefaultRedAlliance = new() {
		Name = "Red Alliance",
		RedColorValue = "255",
		GreenColorValue = "0",
		BlueColorValue = "0"
	};

	private static readonly AllianceEditingData DefaultBlueAlliance = new() {
		Name = "Blue Alliance",
		RedColorValue = "0",
		GreenColorValue = "0",
		BlueColorValue = "255"
	};



	public static readonly BooleanDataFieldEditingData DefaultBooleanDataFieldEditingData = new() {
		Name = "New Data Field",
		InitialValue = false
	};

	public static readonly TextDataFieldEditingData DefaultTextDataFieldEditingData = new() {
		Name = "New Data Field",
		InitialValue = "",
		MustNotBeEmpty = false,
		MustNotBeInitialValue = false
	};

	public static readonly SelectionDataFieldEditingData DefaultSelectionDataFieldEditingData = new() {
		Name = "New Data Field",
		OptionNames = ReadOnlyList.Empty,
		InitialValue = "",
		RequiresValue = true
	};

	public static readonly IntegerDataFieldEditingData DefaultIntegerDataFieldEditingData = new() {
		Name = "New Data Field",
		InitialValue = "0",
		MinValue = $"{int.MinValue}",
		MaxValue = $"{int.MaxValue}",
	};

	public static readonly DataFieldEditingData DefaultDataFieldEditingData = DefaultTextDataFieldEditingData;



	public static readonly InputEditingData DefaultInputEditingData = new() {
		DataFieldName = "",
		Label = ""
	};



	public static GameEditingData DefaultGameEditingData => new() {

		Name = GameNameGenerator.GetRandomGameName(),
		Year = DateTime.Now.Year.ToString(),
		Description = "",

		VersionMajorNumber = "1",
		VersionMinorNumber = "0",
		VersionPatchNumber = "0",
		VersionDescription = "",

		RobotsPerAlliance = "3",
		AlliancesPerMatch = "2",
		Alliances = new List<AllianceEditingData> { DefaultRedAlliance, DefaultBlueAlliance }.ToReadOnly(),
		DataFields = ReadOnlyList.Empty,

		SetupTabInputs = ReadOnlyList.Empty,
		AutoTabInputs = ReadOnlyList.Empty,
		TeleTabInputs = ReadOnlyList.Empty,
		EndgameTabInputs = ReadOnlyList.Empty
	};



	public static void AddUniqueAlliance(this GameEditor gameEditor) {

		gameEditor.Alliances.Add(AllianceGenerator.GenerateUniqueAlliance(gameEditor.Alliances.SelectIfHasValue(x => x.Color.OutputObject)));
	}

}