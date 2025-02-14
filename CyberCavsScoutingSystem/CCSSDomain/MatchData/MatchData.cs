using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using Version = CCSSDomain.GameSpecification.Version;

namespace CCSSDomain.MatchData;



public class MatchData {

	public required string GameName { get; init; }
	public required Version GameVersion { get; init; }
	public required int GameHashCode { get; init; }

	public string? EventCode { get; init; }

	public required Match Match { get; init; }

	public required uint TeamNumber { get; init; }

	public required uint AllianceIndex { get; init; }

	public required DateTime StartTime { get; init; }
	public required DateTime EndTime { get; init; }

	public required ReadOnlyList<object?> DataFields { get; init; }

	// database should have a "has errors" column, errors themselves are stored in a separate table
	public required ReadOnlyList<DomainError> Errors { get; init; } // todo figure out if this is the right type



	public MatchData(
		GameSpec gameSpecification,
		ErrorContext errorContext,
		string? eventCode,
		EventSchedule? eventSchedule,
		Match match,
		uint teamNumber,
		uint allianceIndex,
		DateTime startTime,
		DateTime endTime,
		ReadOnlyList<DataField> dataFields,
		ReadOnlyList<object?> dataCollectionWarnings) {

		List<DomainError> errors = [];

		ValidateMatch(errors.Add, errorContext, match, teamNumber, eventCode, eventSchedule);
		ValidateAllianceIndex(errors.Add, errorContext, gameSpecification, allianceIndex);
		ValidateTimes(errors.Add, errorContext, startTime, endTime);
		ValidateDataFields(errors.Add, errorContext, gameSpecification, dataFields, out ReadOnlyList<object?> dataFieldResults);

		GameName = gameSpecification.Name;
		GameVersion = gameSpecification.Version;
		GameHashCode = gameSpecification.GetHashCode();
		EventCode = eventCode;
		Match = match;
		TeamNumber = teamNumber;
		AllianceIndex = allianceIndex;
		StartTime = startTime;
		EndTime = endTime;
		DataFields = dataFieldResults;
		Errors = errors.ToReadOnly();
	}

	private static void ValidateMatch(
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		Match match,
		uint teamNumber,
		string? eventCode,
		EventSchedule? eventSchedule) {

		if (eventSchedule is null) {
			return;
		}

		if (eventCode is null) {
			errorSink(new EventScheduleButNoEventCode(errorContext));
		}

		// todo this will need to be updated to support other tournament formats
		switch (match.Type) {

			case MatchType.Practice:
				break;

			case MatchType.Qualification:
				uint matchCount = (uint)eventSchedule.QualificationMatches.Count;
				if (match.MatchNumber > matchCount) {
					errorSink(new MatchNumberOutOfRange(errorContext, match.MatchNumber, matchCount, MatchType.Qualification) );
				}
				break;

			case MatchType.DoubleElimination:
				if (match.MatchNumber > 13) {
					errorSink(new MatchNumberOutOfRange(errorContext, match.MatchNumber, 13, MatchType.DoubleElimination));
				}
				break;

			case MatchType.Final:
				if (match.MatchNumber > 3) {
					errorSink(new MatchNumberOutOfRange(errorContext, match.MatchNumber, 3, MatchType.DoubleElimination));
				}
				break;

			default:
				throw new UnreachableException();
		}

		if (!eventSchedule.Teams.Contains(teamNumber)) {
			errorSink(new TeamNotInMatch(errorContext, teamNumber));
		}

		// todo validate start time and end time against event?
	}

	private static void ValidateAllianceIndex(
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		GameSpec gameSpecification,
		uint allianceIndex) {

		if (gameSpecification.Alliances.Count <= allianceIndex) {
			errorSink(new AllianceIndexOutOfRangeError(errorContext, allianceIndex, gameSpecification.AlliancesPerMatch - 1));
		}
	}

	private static void ValidateTimes(
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		DateTime startTime,
		DateTime endTime) {

		if (endTime < startTime) {
			errorSink(new StartTimeAfterEndTime(errorContext, startTime, endTime));
		}
	}

	private static void ValidateDataFields(
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		GameSpec gameSpec,
		ReadOnlyList<DataField> dataFields,
		out ReadOnlyList<object?> dataFieldResults) {

		List<object?> results = [];

		for (int index = 0; index < gameSpec.DataFields.Count; index++) {

			DataFieldSpec dataFieldSpec = gameSpec.DataFields[index];
			DataField dataField = dataFields[index];

			switch (dataFieldSpec) {

				case BooleanDataFieldSpec:

					if (dataField is BooleanDataField booleanDataField) {
						results.Add(booleanDataField);
					} else {
						errorSink(new DataFieldTypeMismatch(errorContext, dataFieldSpec.Name, ))
					}

					booleanDataField.


					switch (dataField) {
							case BooleanDataField booleanDataField:
								results.Add(booleanDataField.Value);
								continue;
							case TextDataField textDataField:
								errorSink(new DataFieldTypeMismatch(errorContext, DataFieldType.Boolean, DataFieldType.Text, textDataField.Text) { Message = $"Where the BooleanDataField '{dataField.Name}' was expected a TextDataField '{textDataField.Name}' with the value '{textDataField.Text}' was received." });
								results.Add(null);
								continue;
							case IntegerDataField integerDataField:
								errors.Add(new() { Message = $"Where the BooleanDataField '{dataField.Name}' was expected a IntegerDataField '{integerDataField.Name}' with the value '{integerDataField.Value}' was received." });
								results.Add(null);
								continue;
							case SelectionDataField selectionDataField:
								errors.Add(new() { Message = $"Where the BooleanDataField '{dataField.Name}' was expected a SelectionDataField '{selectionDataField.Name}' with the value '{selectionDataField.SelectedOption}' was received." });
								results.Add(null);
								continue;
							default:
								throw new UnreachableException();
						}

				case TextDataFieldSpec textDataFieldSpec:
					switch (dataField) {
						case BooleanDataField booleanDataField:
							errors.Add(new() { Message = $"Where the TextDataField '{dataField.Name}' was expected a BooleanDataField '{booleanDataField.Name}' with the value '{booleanDataField.Value}' was received." });
							results.Add(null);
							continue;
						case TextDataField textDataField:
							if (textDataFieldSpec.MustNotBeEmpty && string.IsNullOrEmpty(textDataField.Text)) {
								errors.Add(new() { Message = $"The TextDataField '{dataField.Name}' is empty." });
							}
							results.Add(textDataField.Text);
							continue;
						case IntegerDataField integerDataField:
							errors.Add(new() { Message = $"Where the TextDataField '{dataField.Name}' was expected a IntegerDataField '{integerDataField.Name}' with the value '{integerDataField.Value}' was received." });
							results.Add(null);
							continue;
						case SelectionDataField selectionDataField:
							errors.Add(new() { Message = $"Where the TextDataField '{dataField.Name}' was expected a SelectionDataField '{selectionDataField.Name}' with the value '{selectionDataField.SelectedOption}' was received." });
							results.Add(null);
							continue;
						default:
							throw new UnreachableException();
					}

				case IntegerDataFieldSpec integerDataFieldSpec:
					switch (dataField) {
						case BooleanDataField booleanDataField:
							errors.Add(new() { Message = $"Where the IntegerDataField '{dataField.Name}' was expected a BooleanDataField '{booleanDataField.Name}' with the value '{booleanDataField.Value}' was received." });
							results.Add(null);
							continue;
						case TextDataField textDataField:
							errors.Add(new() { Message = $"Where the IntegerDataField '{dataField.Name}' was expected a TextDataField '{textDataField.Name}' with the value '{textDataField.Text}' was received." });
							results.Add(null);
							continue;
						case IntegerDataField integerDataField:
							errors.Add(new() { Message = $"Where the IntegerDataField '{dataField.Name}' was expected a IntegerDataField '{integerDataField.Name}' with the value '{integerDataField.Value}' was received." });

							if (integerDataField.Value > integerDataField.MaxValue) {
								errors.Add(new() { Message = $"The IntegerDataField '{dataField.Name}' has a value of {integerDataField.Value} when the maximum value is {integerDataFieldSpec.MaxValue}." });
							}

							if (integerDataField.Value < integerDataField.MinValue) {
								errors.Add(new() { Message = $"The IntegerDataField '{dataField.Name}' has a value of {integerDataField.Value} when the minimum value is {integerDataFieldSpec.MinValue}." });
							}

							results.Add(integerDataField.Value);
							continue;
						case SelectionDataField selectionDataField:
							errors.Add(new() { Message = $"Where the IntegerDataField '{dataField.Name}' was expected a SelectionDataField '{selectionDataField.Name}' with the value '{selectionDataField.SelectedOption}' was received." });
							results.Add(null);
							continue;
						default:
							throw new UnreachableException();
					}

				case SelectionDataFieldSpec selectionDataFieldSpec:
					switch (dataField) {
						case BooleanDataField booleanDataField:
							errors.Add(new() { Message = $"Where the SelectionDataField '{dataField.Name}' was expected a BooleanDataField '{booleanDataField.Name}' with the value '{booleanDataField.Value}' was received." });
							results.Add(null);
							continue;
						case TextDataField textDataField:
							errors.Add(new() { Message = $"Where the SelectionDataField '{dataField.Name}' was expected a TextDataField '{textDataField.Name}' with the value '{textDataField.Text}' was received." });
							results.Add(null);
							continue;
						case IntegerDataField integerDataField:
							errors.Add(new() { Message = $"Where the SelectionDataField '{dataField.Name}' was expected a IntegerDataField '{integerDataField.Name}' with the value '{integerDataField.Value}' was received." });
							results.Add(null);
							continue;
						case SelectionDataField selectionDataField:

							if (selectionDataFieldSpec.RequiresValue && selectionDataField.SelectedOption == Optional<string>.NoValue) {
								errors.Add(new() { Message = $"The SelectionDataField \"{dataField.Name}\" requires a value." });
							}

							if (selectionDataField.SelectedOption.HasValue && !selectionDataField.Options.Contains(selectionDataField.SelectedOption.Value)) {
								errors.Add(new() { Message = $"The SelectionDataField \"{dataField.Name}\" does not contain the specified value '{selectionDataField.SelectedOption.Value}'." });
							}

							results.Add(selectionDataField.SelectedOption.HasValue ? selectionDataField.SelectedOption.Value : Optional.NoValue);
							continue;
						default:
							throw new UnreachableException();
					}

				default:
					throw new UnreachableException();
			}
		}

		dataFieldResults = results.ToReadOnly();
	}

}



public class EventScheduleButNoEventCode : DomainError {

	[SetsRequiredMembers]
	public EventScheduleButNoEventCode(ErrorContext errorContext) : base(errorContext) { }

}

public class MatchNumberOutOfRange : DomainError {

	public uint MatchNumber { get; }

	public uint MaxMatchNumber { get; }

	public MatchType MatchType { get; }

	[SetsRequiredMembers]
	public MatchNumberOutOfRange(ErrorContext errorContext, uint matchNumber, uint maxMatchNumber, MatchType matchType) : base(errorContext) {

		MatchNumber = matchNumber;
		MaxMatchNumber = maxMatchNumber;
		MatchType = matchType;
	}

}

public class TeamNotInMatch : DomainError {

	public uint Team { get; }

	[SetsRequiredMembers]
	public TeamNotInMatch(ErrorContext errorContext, uint team) : base(errorContext) {

		Team = team;
		Message = "Team not found in this match";
	}

}

public class AllianceIndexOutOfRangeError : DomainError {

	public uint AllianceIndex { get; }

	public uint MaxAllianceIndex { get; }

	[SetsRequiredMembers]
	public AllianceIndexOutOfRangeError(ErrorContext errorContext, uint allianceIndex, uint maxAllianceIndex) : base(errorContext) {

		AllianceIndex = allianceIndex;
		MaxAllianceIndex = maxAllianceIndex;
		Message = $"An AllianceIndex of {allianceIndex} was specified. The maximum allowed AllianceIndex is {maxAllianceIndex}.";
	}

}

public class StartTimeAfterEndTime : DomainError {

	public DateTime StartTime { get; }

	public DateTime EndTime { get; }

	[SetsRequiredMembers]
	public StartTimeAfterEndTime(ErrorContext errorContext, DateTime startTime, DateTime endTime) : base(errorContext) {

		StartTime = startTime;
		EndTime = endTime;
		Message = $"The start time '{startTime}' is after the end time '{endTime}'";
	}

}

public class DataFieldTypeMismatch : DomainError {

	public DataFieldSpec ExpectedDataField { get; }

	public DataFieldSpec ReceivedDataField { get; }

	public object Value { get; }

	[SetsRequiredMembers]
	public DataFieldTypeMismatch(ErrorContext errorContext, BooleanDataFieldSpec expectedDataField, TextDataFieldSpec receivedDataField, object value) {

		ExpectedDataField = expectedDataField;
		ReceivedDataField = receivedDataField;
		Value = value;
	}

}