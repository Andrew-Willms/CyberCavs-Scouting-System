using CCSSDomain.Data;

namespace CCSSDomain.Serialization;



// todo consider making IDs uints
public record MatchDataDto {

	public required MatchData MatchData { get; init; }

	public required string DeviceId { get; init; }

	public required int RecordId { get; init; }

	public required (string DeviceId, int RecordId)? EditBasedOn { get; init; }

}

public record CreateMatchDataDto {

	public required MatchData MatchData { get; init; }

	public required string DeviceId { get; init; }

	public required (string DeviceId, int RecordId)? EditBasedOn { get; init; }

}