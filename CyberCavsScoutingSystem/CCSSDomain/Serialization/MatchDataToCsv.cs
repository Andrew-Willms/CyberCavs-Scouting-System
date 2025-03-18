using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CCSSDomain.Data;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Optional;

namespace CCSSDomain.Serialization;



public static class MatchDataToCsv {

	public static string GetCsvHeaders(GameSpec gameSpecification) {

		StringBuilder stringBuilder = new("ScoutName,EventCode,MatchNumber,MatchType,ReplayNumber,AllianceIndex,TeamNumber,StartTime,EndTime,");

		stringBuilder.AppendJoin(",", gameSpecification.DataFields.Select(x => x.Name));

		foreach (DataFieldSpec dataField in gameSpecification.DataFields) {
			stringBuilder.Append(dataField.Name.ToCsvFriendly());
			stringBuilder.Append(',');
		}

		return stringBuilder.ToString();
	}

	public static string Serialize(MatchData matchData) {

		StringBuilder stringBuilder = new(matchData.ScoutName.ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EventCode);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.Match.MatchNumber);
		stringBuilder.Append(',');
		stringBuilder.Append((int)matchData.Match.Type);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.Match.ReplayNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.AllianceIndex);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.TeamNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.StartTime.ToString("o").ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EndTime.ToString("o").ToCsvFriendly());

		for (int i = 0; i < matchData.DataFields.Count; i++) {

			switch (matchData.GameSpecification.DataFields[i], matchData.DataFields[i]) {
				case (BooleanDataFieldSpec, bool value): {
					stringBuilder.Append(value ? ",1" : ",0");
					break;
				}
				case (TextDataFieldSpec, string value): {
					stringBuilder.Append(",");
					stringBuilder.Append(value.ToCsvFriendly());
					break;
				}
				case (IntegerDataFieldSpec, int value): {
					stringBuilder.Append(",");
					stringBuilder.Append(value);
					break;
				}
				case (SelectionDataFieldSpec selectionDataFieldSpec, Optional<string> optional): {

					if (!optional.HasValue) {
						stringBuilder.Append(",");
						continue;
					}
					stringBuilder.Append(",");
					stringBuilder.Append(selectionDataFieldSpec.Options.ToList().IndexOf(optional.Value)); // todo lazy af
					break;
				}
				default:
					throw new UnreachableException();
			}
		}

		return stringBuilder.ToString();
	}

	public static MatchData? Deserialize(string matchData, GameSpec gameSpecification) {

		List<string> columns = matchData.SplitTextToCsvColumns();

		if (columns.Count != 9 + gameSpecification.DataFields.Count) {
			return null;
		}

		bool success = Enum.TryParse(columns[3], out MatchType type);
		success &= uint.TryParse(columns[2], out uint matchNumber);
		success &= uint.TryParse(columns[4], out uint replayNumber);
		success &= uint.TryParse(columns[6], out uint teamNumber);
		success &= uint.TryParse(columns[5], out uint allianceIndex);
		success &= DateTime.TryParse(columns[7], out DateTime startTime);
		success &= DateTime.TryParse(columns[8], out DateTime endTime);

		if (!success) {
			return null;
		}

		List<object> dataFieldValues = [];
		for (int i = 0; i < gameSpecification.DataFields.Count; i++) {

			string value = columns[i + 9];

			switch (gameSpecification.DataFields[i]) {

				case BooleanDataFieldSpec:
					switch (value) {
						case "1":
							dataFieldValues.Add(true);
							continue;
						case "0":
							dataFieldValues.Add(false);
							continue;
						default:
							return null;
					}

				case TextDataFieldSpec:
					dataFieldValues.Add(value);
					break;

				case IntegerDataFieldSpec: {
					if (!int.TryParse(value, out int result)) {
						return null;
					}
					dataFieldValues.Add(result);
					break;
				}
				case SelectionDataFieldSpec selectionSpec: {

					if (value == string.Empty) {
						dataFieldValues.Add(Optional.NoValue);
						break;
					}

					if (!int.TryParse(value, out int result)) {
						return null;
					}

					if (result < 0 || result >= selectionSpec.Options.Count) {
						return null;
					}

					dataFieldValues.Add(selectionSpec.Options[result].Optionalize());
					break;
				}
			}
		}

		return MatchData.FromRaw(
			gameSpecification: gameSpecification,
			eventCode: columns[1] == string.Empty ? null : columns[1],
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
			dataFieldValues.ToReadOnly()
		);
	}

}