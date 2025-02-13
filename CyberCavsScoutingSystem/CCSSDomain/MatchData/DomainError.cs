using System;
using System.Diagnostics.CodeAnalysis;
using CCSSDomain.GameSpecification;

namespace CCSSDomain.MatchData;



public class ErrorContext {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

}

public class DomainError {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

	public required DateTime TimeCreated { get; init; }

	public DomainError() { }

	[SetsRequiredMembers]
	public DomainError(ErrorContext errorContext) {
		DeviceId = errorContext.DeviceId;
		DeviceName = errorContext.DeviceName;
		TimeCreated = DateTime.Now;
	}

}

public class StartTimeAfterEndTime : DomainError {

	public required DateTime StartTime { get; init; }

	public required DateTime EndTime { get; init; }
}