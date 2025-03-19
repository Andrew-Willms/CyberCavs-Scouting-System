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

		stringBuilder.AppendJoin(",", gameSpecification.DataFields.Select(x => x.Name.ToCsvFriendly()));

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

		bool success = uint.TryParse(columns[2], out uint matchNumber);
		success &= Enum.TryParse(columns[3], out MatchType type);
		success &= uint.TryParse(columns[4], out uint replayNumber);
		success &= uint.TryParse(columns[5], out uint allianceIndex);
		success &= uint.TryParse(columns[6], out uint teamNumber);
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

public static class MatchDataDtoToCsv {

	public static string GetCsvHeaders(GameSpec gameSpecification) {

		StringBuilder stringBuilder = new("DeviceId,RecordId,EditDeviceId,EditRecordId,ScoutName,EventCode,MatchNumber,MatchType,ReplayNumber,AllianceIndex,TeamNumber,StartTime,EndTime,");

		stringBuilder.AppendJoin(",", gameSpecification.DataFields.Select(x => x.Name.ToCsvFriendly()));

		return stringBuilder.ToString();
	}

	public static string Serialize(MatchDataDto matchData) {

		StringBuilder stringBuilder = new(matchData.DeviceId);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.RecordId);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EditBasedOn?.DeviceId);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.EditBasedOn?.RecordId);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.ScoutName.ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.EventCode);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.Match.MatchNumber);
		stringBuilder.Append(',');
		stringBuilder.Append((int)matchData.MatchData.Match.Type);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.Match.ReplayNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.AllianceIndex);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.TeamNumber);
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.StartTime.ToString("o").ToCsvFriendly());
		stringBuilder.Append(',');
		stringBuilder.Append(matchData.MatchData.EndTime.ToString("o").ToCsvFriendly());

		for (int i = 0; i < matchData.MatchData.DataFields.Count; i++) {

			switch (matchData.MatchData.GameSpecification.DataFields[i], matchData.MatchData.DataFields[i]) {
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

	public static MatchDataDto? Deserialize(string matchData, GameSpec gameSpecification) {

		List<string> columns = matchData.SplitTextToCsvColumns();

		if (columns.Count != 13 + gameSpecification.DataFields.Count) {
			return null;
		}

		bool success = int.TryParse(columns[1], out int recordId);
		success &= uint.TryParse(columns[6], out uint matchNumber);
		success &= Enum.TryParse(columns[7], out MatchType type);
		success &= uint.TryParse(columns[8], out uint replayNumber);
		success &= uint.TryParse(columns[9], out uint allianceIndex);
		success &= uint.TryParse(columns[10], out uint teamNumber);
		success &= DateTime.TryParse(columns[11], out DateTime startTime);
		success &= DateTime.TryParse(columns[12], out DateTime endTime);

		if (!success) {
			return null;
		}

		(string, int)? editBasedOn;
		switch (columns[2] == string.Empty, columns[3] == string.Empty) {
			case (true, true):
				editBasedOn = null;
				break;
			case (false, false):
				if (!int.TryParse(columns[3], out int editRecordId)) {
					return null;
				}
				editBasedOn = (columns[2], editRecordId);
				break;
			default:
				return null;
		}

		List<object> dataFieldValues = [];
		for (int i = 0; i < gameSpecification.DataFields.Count; i++) {

			string value = columns[i + 13];

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

		MatchData? matchDataObject = MatchData.FromRaw(
			gameSpecification: gameSpecification,
			eventCode: columns[5] == string.Empty ? null : columns[5],
			eventSchedule: null,
			scoutName: columns[4],
			match: new() {
				MatchNumber = matchNumber,
				ReplayNumber = replayNumber,
				Type = type
			},
			teamNumber,
			allianceIndex,
			startTime,
			endTime,
			dataFieldValues.ToReadOnly());

		if (matchDataObject is null) {
			return null;
		}

		return new() {
			MatchData = matchDataObject,
			DeviceId = columns[0],
			RecordId = recordId,
			EditBasedOn = editBasedOn
		};
	}

}