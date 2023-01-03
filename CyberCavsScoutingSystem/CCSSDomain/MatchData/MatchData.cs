using CCSSDomain.GameSpecification;

namespace CCSSDomain.MatchData; 



public class MatchData {
	
	public required AllianceSpec Alliance { get; init; }

	public uint ReplayNumber { get; init; } = 0;
	public bool IsReplay => ReplayNumber > 0;

	public MatchData(GameSpec gameSpec) {

	}

}