using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using CCSSDomain.Data;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Optional;

namespace CCSSDomain.Serialization;



public static class MatchDataToCsv {

	public static string GetCsvHeaders(GameSpec gameSpecification) {

		// todo
		return """ScoutName,EventCode,MatchNumber,MatchType,ReplayNumber,AllianceIndex,TeamNumber,StartTime,EndTime,"Auto L1 Coral","Auto L2 Coral","Auto L3 Coral","Auto L4 Coral","Auto Algae Net","Auto Algae Processor","Tele L1 Coral","Tele L2 Coral","Tele L3 Coral","Tele L4 Coral","Tele Algae Net","Tele Algae Processor","Climb","Disconnected","Defense","Comments""";
	}

	public static string Serialize(MatchData matchData) {

		StringBuilder stringBuilder = new(matchData.ScoutName.ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EventCode);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.Match.MatchNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.Match.Type);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.Match.ReplayNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.AllianceIndex);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.TeamNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.StartTime.ToString(CultureInfo.InvariantCulture).ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EndTime.ToString(CultureInfo.InvariantCulture).ToCsvFriendly());
		stringBuilder.Append(',');

		for (int i = 0; i < matchData.DataFields.Count; i++) {

			if (matchData.GameSpecification.DataFields[i] is SelectionDataFieldSpec selectionDataFieldSpec) {

				Optional<string> optional = matchData.DataFields[i] as Optional ?? throw new UnreachableException();

				if (!optional.HasValue) {
					stringBuilder.Append(",");
					continue;
				}

				int j;
				for (j = 0; j < selectionDataFieldSpec.Options.Count; j++) {
					if (selectionDataFieldSpec.Options[j] == optional.Value) {
						break;
					}
				}

				if (j >= selectionDataFieldSpec.Options.Count) {
					throw new UnreachableException();
				}

				stringBuilder.Append(j);
				stringBuilder.Append(",");
				continue;
			}

			stringBuilder.Append(matchData.DataFields[i]?.ToString()?.ToCsvFriendly());
			stringBuilder.Append(",");
		}

		stringBuilder.Remove(stringBuilder.Length - 1, 1);

		return stringBuilder.ToString();
	}

	public static MatchData? Deserialize(string matchData, GameSpec gameSpecification) {

		List<string> columns = matchData.SplitTextToCsvColumns();

		if (columns.Count != 9 + gameSpecification.DataFields.Count) {
			return null;
		}

		bool success = Enum.TryParse(columns[4], out MatchType type);
		success &= uint.TryParse(columns[2], out uint matchNumber);
		success &= uint.TryParse(columns[3], out uint replayNumber);
		success &= uint.TryParse(columns[6], out uint teamNumber);
		success &= uint.TryParse(columns[5], out uint allianceIndex);
		success &= DateTime.TryParse(columns[7][1..^2], out DateTime startTime);
		success &= DateTime.TryParse(columns[8][1..^2], out DateTime endTime);

		List<DataField> dataFieldValues = [];
		for (int i = 0; i < gameSpecification.DataFields.Count; i++) {

			string value = columns[i + 9];

			switch (gameSpecification.DataFields[i]) {

				case BooleanDataFieldSpec booleanSpec: {

					if (!bool.TryParse(value, out bool result)) {
						return null;
					}

					// TODO MatchData class should consume something like a DataFieldResult class?
					// don't need to be checking if the data is valid once the match is over
					BooleanDataField booleanDataField = new(booleanSpec) {
						Value = result
					};

					if (booleanDataField.Errors.Any()) {
						return null;
					}

					dataFieldValues.Add(booleanDataField);
					break;
				}
				case TextDataFieldSpec textSpec: {

					TextDataField textDataField = new(textSpec) {
						Value = value
					};

					if (textDataField.Errors.Any()) {
						return null;
					}

					dataFieldValues.Add(textDataField);
					break;
				}
				case IntegerDataFieldSpec integerSpec: {

					if (!int.TryParse(value, out int result)) {
						return null;
					}

					IntegerDataField integerDataField = new(integerSpec) {
						Value = result
					};

					if (integerDataField.Errors.Any()) {
						return null;
					}

					dataFieldValues.Add(integerDataField);
					break;
				}
				case SelectionDataFieldSpec selectionSpec: {

					if (!int.TryParse(value, out int result)) {
						return null;
					}

					if (result < 0 || result >= selectionSpec.Options.Count) {
						return null;
					}

					SelectionDataField selectionDataField = new(selectionSpec) {
						Value = value == string.Empty ? Optional<string>.NoValue : selectionSpec.Options[result].Optionalize()
					};

					if (selectionDataField.Errors.Any()) {
						return null;
					}

					dataFieldValues.Add(selectionDataField);
					break;
				}
			}
		}

		ReadOnlyList<DataField> readOnlyDataFields = dataFieldValues.ToReadOnly();

		if (!success) {
			return null;
		}

		return new(
			errorContext: null!,
			gameSpecification: gameSpecification,
			eventCode: columns[1],
			eventSchedule: null,
			scoutName: columns[0],
			match: new() {
				MatchNumber = matchNumber,
				ReplayNumber = replayNumber,
				Type = type
			},
			teamNumber,
			allianceIndex,
			startTime,
			endTime,
			readOnlyDataFields
		);
	}

}