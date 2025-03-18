using CCSSDomain.Data;

namespace CcssDomain.Tests.Serialization;



public class MatchDataToCsvTests {

	[Theory]
	[ClassData(typeof(SampleData))]
	public void TestSerialization(MatchData matchData) {
		
	}

}