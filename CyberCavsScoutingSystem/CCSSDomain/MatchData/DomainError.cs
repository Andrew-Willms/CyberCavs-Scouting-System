using System;
using System.Diagnostics.CodeAnalysis;

namespace CCSSDomain.MatchData;



public class ErrorContext {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

}

public class DomainError {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

	public required DateTime TimeCreated { get; init; }

	public DomainError(string deviceId, string deviceName) {
		DeviceId = deviceId;
		DeviceName = deviceName;
	}

	[SetsRequiredMembers]
	public DomainError(ErrorContext errorContext) {
		DeviceId = errorContext.DeviceId;
		DeviceName = errorContext.DeviceName;
		TimeCreated = DateTime.Now;
	}

}