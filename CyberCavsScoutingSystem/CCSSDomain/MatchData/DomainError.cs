using System;
using CCSSDomain.GameSpecification;

namespace CCSSDomain.MatchData;



public class DomainError {

	public /*required*/ string DeviceId { get; init; }

	public /*required*/ string DeviceName { get; init; }

	public /*required*/ string Message { get; init; }
}

public class MatchNumberOutOfRange : DomainError {

	public required MatchType MatchType { get; init; }

	public required MatchType MaxMatchNumber { get; init; }

	public required MatchType MatchNumber { get; init; }

}

public class AllianceDoesNotMatch : DomainError {

	public required AllianceColor AllianceColor { get; init; }

}

public class StartTimeAfterEndTime : DomainError {

	public required DateTime StartTime { get; init; }

	public required DateTime EndTime { get; init; }
}

