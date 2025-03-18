using CCSSDomain.Data;

namespace CCSSDomain.Protocols;



public class MatchDataDto {

	public required MatchData MatchData { get; init; }

	public required string DeviceId { get; init; }

	public required int UnifiedRecordId { get; init; }

	public required int? EditBasedOn { get; init; }

}