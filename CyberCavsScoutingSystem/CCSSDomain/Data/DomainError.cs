using System;
using System.Diagnostics.CodeAnalysis;

namespace CCSSDomain.Data;



public class ErrorContext {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

	public required string Scout { get; init; }

}

public class DomainError {

	public required string DeviceId { get; init; }

	public required string DeviceName { get; init; }

	public required string Scout { get; init; }

	public required DateTime TimeCreated { get; init; }

	public DomainError() { }

	[SetsRequiredMembers]
	public DomainError(string deviceId, string deviceName, string scout) {
		DeviceId = deviceId;
		DeviceName = deviceName;
		Scout = scout;
		TimeCreated = DateTime.Now;
	}

	[SetsRequiredMembers]
	public DomainError(string deviceId, string deviceName, string scout, DateTime timeCreated) {
		DeviceId = deviceId;
		DeviceName = deviceName;
		Scout = scout;
		TimeCreated = timeCreated;
	}

	[SetsRequiredMembers]
	public DomainError(ErrorContext errorContext) {
		DeviceId = errorContext.DeviceId;
		DeviceName = errorContext.DeviceName;
		Scout = errorContext.Scout;
		TimeCreated = DateTime.Now;
	}

}