using System.ComponentModel;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.SimpleEvent;

namespace CCSSDomain.MatchData; 



public class MatchData {
	
	public required Alliance Alliance { get; init; }

	public uint ReplayNumber { get; init; }
	public bool IsReplay => ReplayNumber > 0;

	public required string Comments { get; set; }




	public MatchData(GameSpecification.GameSpec gameSpecSpecification) {

	}



}