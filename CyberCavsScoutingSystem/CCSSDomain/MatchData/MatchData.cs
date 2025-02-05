using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using Version = CCSSDomain.GameSpecification.Version;
using UtilitiesLibrary.Optional;

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

		errors.AddRange(ValidateMatch(match, teamNumber, eventCode, eventSchedule));
		errors.AddRange(ValidateAllianceIndex(gameSpecification, allianceIndex));
		errors.AddRange(ValidateTimes(startTime, endTime));
		errors.AddRange(ValidateDataFields(gameSpecification, dataFields, out ReadOnlyList<object?> dataFieldResults));

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

	private static ReadOnlyList<DomainError> ValidateMatch(Match match, uint teamNumber, string? eventCode, EventSchedule? eventSchedule) {

		List<DomainError> errors = [];

		if (eventSchedule is null) {
			return errors.ToReadOnly();
		}

		if (eventCode is null) {
			errors.Add(new() { Message = "EventSchedule but no event code" });
		}

		// todo this will need to be updated to support other tournament formats
		switch (match.Type) {

			case MatchType.Practice:
				break;

			case MatchType.Qualification:
				if (match.MatchNumber > eventSchedule.Matches.Count) {
					errors.Add(new() { Message = "Match number too high" });
				}
				break;

			case MatchType.DoubleElimination:
				if (match.MatchNumber > 13) {
					errors.Add(new() { Message = "Elimination match number too high" });
				}
				break;

			case MatchType.Final:
				if (match.MatchNumber > 3) {
					errors.Add(new() { Message = "Final match number too high" });
				}
				break;

			default:
				throw new UnreachableException();
		}

		// todo validate start time and end time against event?

		if (!eventSchedule.Teams.Contains(teamNumber)) {
			errors.Add(new() { Message = "Team not found in this match" });
		}

		return errors.ToReadOnly();
	}

	private static ReadOnlyList<DomainError> ValidateAllianceIndex(GameSpec gameSpecification, uint allianceIndex) {

		List<DomainError> errors = [];

		if (gameSpecification.Alliances.Count <= allianceIndex) {
			errors.Add(new() { Message = "AllianceIndex too high" });
		}

		return errors.ToReadOnly();
	}

	private static ReadOnlyList<DomainError> ValidateTimes(DateTime startTime, DateTime endTime) {

		List<DomainError> errors = [];

		if (endTime < startTime) {
			errors.Add(new StartTimeAfterEndTime { StartTime = startTime, EndTime = endTime });
		}

		return errors.ToReadOnly();
	}

	private static ReadOnlyList<DomainError> ValidateDataFields(GameSpec gameSpec, ReadOnlyList<DataField> dataFields, out ReadOnlyList<object?> dataFieldResults) {

		List<DomainError> errors = [];
		List<object?> results = [];

		for (int index = 0; index < gameSpec.DataFields.Count; index++) {

			DataFieldSpec dataFieldSpec = gameSpec.DataFields[index];
			DataField dataField = dataFields[index];

			switch (dataFieldSpec) {

				case BooleanDataFieldSpec:
					switch (dataField) {
						case BooleanDataField booleanDataField:
							results.Add(booleanDataField.Value);
							continue;
						case TextDataField textDataField:
							errors.Add(new() { Message = $"Where the BooleanDataField '{dataField.Name}' was expected a TextDataField '{textDataField.Name}' with the value '{textDataField.Text}' was received." });
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
		return errors.ToReadOnly();
	}

}