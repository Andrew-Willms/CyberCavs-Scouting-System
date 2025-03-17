using CCSSDomain.Data;
using CCSSDomain.GameSpecification;

namespace Database;



public readonly record struct DataToSend {

	public required List<GameSpec> GameSpecifications { get; init; }

	public required List<EventSchedule> EventSchedules { get; init; }

	public required List<MatchData> MatchData { get; init; }

}