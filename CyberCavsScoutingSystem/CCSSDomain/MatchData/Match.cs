namespace CCSSDomain.MatchData;



public enum MatchType {
	Practice,
	Qualification,
	Elimination,
	//QuarterFinal, // Todo add support for old style playoffs and custom tournament formats
	//SemiFinal,
	Final
}

public class Match {

	public required uint MatchNumber { get; init; }

	public required uint ReplayNumber { get; init; }
	
	public required MatchType Type { get; init; }

}