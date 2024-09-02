using System;
using CCSSDomain.MatchData;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.GameSpecification;



public class EventCode {

	public required string Value { get; init; }

}



public class ScheduledMatch {

	public required ReadOnlyList<Alliance> Alliances { get; init; }

	public DateTime? Time { get; init; }

}

public class EventInfo {

	public required string Name { get; init; }

	public required string EventCode { get; init; }

	public required DateTime StartDate { get; init; }

	public required DateTime EndDate { get; init; }

	public required ReadOnlyList<uint> Teams { get; init; }

	public required ReadOnlyList<ScheduledMatch> Matches { get; init; }

}

// TODO add support for custom event formats, non-3v3s etc
// maybe an enum with Regular, Old, and Custom values  