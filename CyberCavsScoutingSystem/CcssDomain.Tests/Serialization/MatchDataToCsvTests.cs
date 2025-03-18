using CCSSDomain.Data;
using CCSSDomain.Serialization;

namespace CcssDomain.Tests.Serialization;



public class MatchDataToCsvTests {

	[Theory]
	[ClassData(typeof(SampleData))]
	public void TestSerialization(MatchData matchData) {

		string serialized = MatchDataToCsv.Serialize(matchData);
		MatchData? deserialized = MatchDataToCsv.Deserialize(serialized, SampleData.GameSpec);

		Assert.True(matchData.Equals(deserialized));
	}

}