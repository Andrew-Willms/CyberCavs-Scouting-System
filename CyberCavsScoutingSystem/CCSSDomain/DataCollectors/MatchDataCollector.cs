﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Optional;

namespace CCSSDomain.DataCollectors; 



public class MatchDataCollector {

	public GameSpec GameSpecification { get; }

	public Optional<uint> MatchNumber { get; set; } = Optional.NoValue;
	public Optional<uint> ReplayNumber { get; set; } = 0u.Optionalize();
	public Optional<bool> IsPlayoff { get; set; } = false.Optionalize();

	public Optional<uint> TeamNumber { get; set; } = Optional.NoValue;

	public Optional<Alliance> Alliance { get; set; } = Optional.NoValue;

	public DateTime Time { get; } = DateTime.Now;

	public ReadOnlyList<DataField> DataFields { get; }

	public ReadOnlyList<InputDataCollector> SetupTabInputs { get; }
	public ReadOnlyList<InputDataCollector> AutoTabInputs { get; }
	public ReadOnlyList<InputDataCollector> TeleTabInputs { get; }
	public ReadOnlyList<InputDataCollector> EndgameTabInputs { get; }



	public MatchDataCollector(GameSpec gameSpecification) {

		GameSpecification = gameSpecification;

		DataFields = GameSpecification.DataFields.Select(DataField.FromSpec).ToReadOnly();

		SetupTabInputs = GameSpecification.SetupTabInputs.Select(x => InputDataCollector.FromDataField(x, DataFields.Single(xx => xx.Name == x.DataFieldName))).ToReadOnly();
        AutoTabInputs = GameSpecification.AutoTabInputs.Select(x => InputDataCollector.FromDataField(x, DataFields.Single(xx => xx.Name == x.DataFieldName))).ToReadOnly();
        TeleTabInputs = GameSpecification.TeleTabInputs.Select(x => InputDataCollector.FromDataField(x, DataFields.Single(xx => xx.Name == x.DataFieldName))).ToReadOnly();
        EndgameTabInputs = GameSpecification.EndgameTabInputs.Select(x => InputDataCollector.FromDataField(x, DataFields.Single(xx => xx.Name == x.DataFieldName))).ToReadOnly();
    }

	public string ConvertDataToCsv(string scout, string @event) {

        StringBuilder matchData = new(
            $"{scout.ToCsvFriendly()}," +
            $"{@event.ToCsvFriendly()}," +
            $"{MatchNumber.Value.ToString().ToCsvFriendly()}," +
            $"{ReplayNumber.Value.ToString().ToCsvFriendly()}," +
            $"{IsPlayoff.Value.ToString().ToCsvFriendly()}," +
            $"{Alliance.Value.Name.ToCsvFriendly()}," +
			$"{TeamNumber.Value.ToString().ToCsvFriendly()}," +
            $"{Time.ToString(CultureInfo.InvariantCulture).ToCsvFriendly()},");

        foreach (DataField dataField in DataFields) {

            switch (dataField) {

                case TextDataField textDataField:
                    matchData.Append($"{textDataField.Text.ToCsvFriendly()},");
                    break;

                case IntegerDataField integerDataField:
                    matchData.Append($"{integerDataField.Value.ToString().ToCsvFriendly()},");
                    break;

                case SelectionDataField selectionDataField:
                    matchData.Append($"{selectionDataField.SelectedOption.Value.ToCsvFriendly()},");
                    break;

                default:
                    throw new UnreachableException();
            }
        }

        return matchData.ToString();
    }

}