using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using Version = CCSSDomain.GameSpecification.Version;

namespace CCSSDomain.MatchData;



public class MatchData {

	public string GameName { get; }

	public Version GameVersion { get; }

	public HashCode GameHashCode { get; }

	public required string? EventCode { get; init; }

	public required Match Match { get; init; }

	public required uint TeamNumber { get; init; }

	public required uint AllianceIndex { get; init; }

	public required DateTime StartTime { get; init; }
	public required DateTime EndTime { get; init; }

	public required ReadOnlyList<DataFieldResult> DataFields { get; init; }

	public required ReadOnlyList<object> DataCollectionWarnings { get; init; } // todo figure out the type for this

	private MatchData() { }


	public static IResult<MatchData> Create(
		out ReadOnlyList<DomainError> errors,
		GameSpec gameSpecification,
		string? eventCode,
		EventSchedule? eventSchedule,
		Match match,
		uint teamNumber,
		AllianceColor allianceColor,
		DateTime startTime,
		DateTime endTime,
		ReadOnlyList<DataFieldResult> data,
		ReadOnlyList<object> dataCollectionWarnings) {

		List<DomainError> domainErrors = [];

		// todo validate match

		// Validation that happens if there is event info
		if (eventSchedule is not null) {

			// todo this will need to be updated to support other tournament formats
			switch (match.Type) {

				case MatchType.Practice:
					break;

				case MatchType.Qualification:
					if (match.MatchNumber > eventSchedule.Matches.Count) {
						throw new NotImplementedException();
					}
					break;

				case MatchType.DoubleElimination:
					if (match.MatchNumber > 13) {
						throw new NotImplementedException();
					}
					break;

				case MatchType.Final:
					if (match.MatchNumber > 3) {
						throw new NotImplementedException();
					}
					break;

				default:
					throw new UnreachableException();
			}

			if (!eventSchedule.Teams.Contains(teamNumber)) {
				throw new NotImplementedException();
			}

			//if (match.MatchNumber < eventInfo.Matches.Count
			//    && eventInfo.Matches[match.MatchNumber]) {
			//	// idk what I was thinking here
			//}

		}

		if (!gameSpecification.Alliances.Contains(allianceColor)) {
			domainErrors.Add(new AllianceDoesNotMatch { AllianceColor = allianceColor });
		}

		if (endTime < startTime) {
			domainErrors.Add(new StartTimeAfterEndTime { StartTime = startTime, EndTime = endTime});
		}

		for (int i = 0; i < gameSpecification.DataFields.Count; i++) {

		}

		errors = domainErrors.ToReadOnly();

		return new IResult<MatchData>.Success {
			Value = new() { 
				GameSpecification = gameSpecification, 
				EventCode = eventCode,
				Match = match,
				TeamNumber = teamNumber,
				AllianceColor = allianceColor,
				StartTime = startTime,
				EndTime = endTime,
				DataFields = data,
				DataCollectionWarnings = dataCollectionWarnings
			}
		};
	}

}