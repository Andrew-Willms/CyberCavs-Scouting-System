using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.MatchData;



public class Alliance {

	public required AllianceColor Color { get; init; }

	public required ReadOnlyList<uint> Teams { get; init; }

}