﻿using System;
using System.Linq;
using System.Text;
using CCSSDomain.DataCollectors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Results;

namespace CCSSDomain.GameSpecification;



public class GameSpec {

	public required Version Version { get; init; } = new(1, 0, 0);
	//public DateTime VersionReleaseDate { get; } = DateTime.Now;
	//public HashCode Hash ????

	public required string Name { get; init; }
	public string Description { get; init; } = "";
	public required int Year { get; init; }

	public required uint RobotsPerAlliance { get; init; }
	public required uint AlliancesPerMatch { get; init; }

	public required ReadOnlyList<AllianceColor> Alliances { get; init; }

	public required ReadOnlyList<DataFieldSpec> DataFields { get; init; }

	public required ReadOnlyList<InputSpec> SetupTabInputs { get; init; }
	public required ReadOnlyList<InputSpec> AutoTabInputs { get; init; }
	public required ReadOnlyList<InputSpec> TeleTabInputs { get; init; }
	public required ReadOnlyList<InputSpec> EndgameTabInputs { get; init; }



	private GameSpec() { }

	public static IResult<GameSpec> Create(
		string name,
		int year,
		string description,
		Version version,
		uint robotsPerAlliance,
		uint alliancesPerMatch,
		ReadOnlyList<AllianceColor> alliances,
		ReadOnlyList<DataFieldSpec> dataFields,
		ReadOnlyList<InputSpec> setupTabInputs,
		ReadOnlyList<InputSpec> autoTabInputs,
		ReadOnlyList<InputSpec> teleTabInputs,
		ReadOnlyList<InputSpec> endgameTabInputs) {

		foreach (AllianceColor alliance in alliances) {

			if (alliances.Where(x => x != alliance).Any(x => x.Name == alliance.Name)) {
				return new IResult<GameSpec>.Error($"There are multiple alliances with the name '{nameof(alliance.Name)}'.");
			}
		}

		foreach (InputSpec input in setupTabInputs) {

			if (!dataFields.Select(x => x.Name).Contains(input.DataFieldName)) {

				return new IResult<GameSpec>.Error($"Input '{input.Label}' from {nameof(SetupTabInputs)} targets the DataField with the " +
												   $"name '{input.DataFieldName}' but no DataField of that name was found.");
			}
		}

		foreach (InputSpec input in autoTabInputs) {

			if (!dataFields.Select(x => x.Name).Contains(input.DataFieldName)) {

				return new IResult<GameSpec>.Error($"Input '{input.Label}' from {nameof(AutoTabInputs)} targets the DataField with the " +
												   $"name '{input.DataFieldName}' but no DataField of that name was found.");
			}
		}

		foreach (InputSpec input in teleTabInputs) {

			if (!dataFields.Select(x => x.Name).Contains(input.DataFieldName)) {

				return new IResult<GameSpec>.Error($"Input '{input.Label}' from {nameof(TeleTabInputs)} targets the DataField with the " +
												   $"name '{input.DataFieldName}' but no DataField of that name was found.");
			}
		}

		foreach (InputSpec input in endgameTabInputs) {

			if (!dataFields.Select(x => x.Name).Contains(input.DataFieldName)) {

				return new IResult<GameSpec>.Error($"Input '{input.Label}' from {nameof(EndgameTabInputs)} targets the DataField with the " +
												   $"name '{input.DataFieldName}' but no DataField of that name was found.");
			}
		}

		return new IResult<GameSpec>.Success {
			Value = new() {
				Name = name,
				Description = description,
				Year = year,
				Version = version,
				RobotsPerAlliance = robotsPerAlliance,
				AlliancesPerMatch = alliancesPerMatch,
				Alliances = alliances,
				DataFields = dataFields.ToReadOnly(),
				SetupTabInputs = setupTabInputs,
				AutoTabInputs = autoTabInputs,
				TeleTabInputs = teleTabInputs,
				EndgameTabInputs = endgameTabInputs
			}
		};
	}

	// todo probably move somewhere else
	public string GetCsvHeaders() {

		StringBuilder columnHeaders = new(
			$"{"Scout".ToCsvFriendly()}," +
			$"{"Event".ToCsvFriendly()}," +
			$"{nameof(MatchDataCollector.MatchNumber).ToCsvFriendly()}," +
			$"{nameof(MatchDataCollector.ReplayNumber).ToCsvFriendly()}," +
			$"{nameof(MatchDataCollector.IsPlayoff).ToCsvFriendly()}," +
			$"{nameof(MatchDataCollector.Alliance).ToCsvFriendly()}," +
			$"{nameof(MatchDataCollector.TeamNumber).ToCsvFriendly()},"// +
			//$"{nameof(MatchDataCollector.Time).ToCsvFriendly()}," // todo figure this out
		);

		DataFields.Foreach(x => columnHeaders.Append($"{x.Name.ToCsvFriendly()},"));

		return columnHeaders.ToString();
	}

}