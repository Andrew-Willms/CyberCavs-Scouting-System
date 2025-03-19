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

	[Theory]
	[ClassData(typeof(SampleData))]
	public void TestDtoSerialization(MatchData matchData) {

		MatchDataDto matchDataDto = new() {
			MatchData = matchData,
			DeviceId = "deviceId",
			RecordId = 1,
			EditBasedOn = null
		};

		string serialized = MatchDataDtoToCsv.Serialize(matchDataDto);
		MatchDataDto? deserialized = MatchDataDtoToCsv.Deserialize(serialized, SampleData.GameSpec);

		Assert.True(matchDataDto.Equals(deserialized));
	}

	[Theory]
	[ClassData(typeof(SampleData))]
	public void TestDtoSerialization_2(MatchData matchData) {

		MatchDataDto matchDataDto = new() {
			MatchData = matchData,
			DeviceId = "deviceId",
			RecordId = 2,
			EditBasedOn = ("deviceId", 1)
		};

		string serialized = MatchDataDtoToCsv.Serialize(matchDataDto);
		MatchDataDto? deserialized = MatchDataDtoToCsv.Deserialize(serialized, SampleData.GameSpec);

		Assert.True(matchDataDto.Equals(deserialized));
	}

}