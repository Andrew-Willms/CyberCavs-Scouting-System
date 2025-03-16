using System.Collections.Generic;
using CCSSDomain.Data;
using UtilitiesLibrary.MiscExtensions;

namespace CCSSDomain.Protocols;



public class MatchDataProtocolV1 {

	public static string Serialize(MatchData matchData) {



	}

	public static MatchData? Deserialize(string matchData) {

		List<string> columns = matchData.SplitTextToCsvColumns();


	}

}