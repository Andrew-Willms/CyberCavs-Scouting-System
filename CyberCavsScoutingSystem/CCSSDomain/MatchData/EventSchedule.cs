using System;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.MatchData;



public class EventSchedule {

	public required string Name { get; init; }

	public required string EventCode { get; init; }

	public required DateTime StartDate { get; init; }

	public required DateTime EndDate { get; init; }

	public required ReadOnlyList<uint> Teams { get; init; }

	public required ReadOnlyList<ScheduledMatch> Matches { get; init; }

	// TODO add support for custom event formats, non-3v3s etc.
	// maybe an enum with Regular, Old, and Custom values
	// for now all events are considered to have standard double elimination playoff brackets
}



public class ScheduledMatch {

	public required ReadOnlyList<Alliance> Alliances { get; init; }

	public DateTime? Time { get; init; }

}