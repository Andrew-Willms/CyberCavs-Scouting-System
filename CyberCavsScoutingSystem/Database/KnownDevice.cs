namespace Database;



public readonly record struct KnownDevice {

	public required string DeviceId { get; init; }

	public required int IdOfLatestRecord { get; init; }

}