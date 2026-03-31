using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using OneOf;
using OneOf.Types;

namespace Database;



[GenerateOneOf]
public partial class AddNewMatchDataResult : OneOfBase<Success, Exception>;

[GenerateOneOf]
public partial class GetMatchDataResult : OneOfBase<List<MatchDataDto>, Exception, MatchDataDeserializationError, InvalidEditIdsError>;

public class MatchDataDeserializationError {
	public required string SerializedMatchData { get; init; }
};

public class InvalidEditIdsError {

	public string? EditOfRecordFromDevice { get; }

	public int? EditOfRecord { get; }

	public InvalidEditIdsError(string editOfRecordFromDevice) {
		EditOfRecordFromDevice = editOfRecordFromDevice;
		EditOfRecord = null;
	}

	public InvalidEditIdsError(int editOfRecord) {
		EditOfRecordFromDevice = null;
		EditOfRecord = editOfRecord;
	}

};


public interface IDataStore {

	public Task<bool> ConnectAndEnsureTables(string dbPath);



	public Task<List<GameSpec>> GetGameSpecs();

	//public Task<bool> AddGameSpec();



	public Task<GetMatchDataResult> GetMatchData();

	public Task<AddNewMatchDataResult> AddNewMatchData(CreateMatchDataDto matchData);

	public enum AddMatchDataResult {
		Success,
		Duplicate,
		Other
	}

	public Task<AddMatchDataResult> AddMatchDataFromOtherDevice(MatchDataDto matchData);

	public Task<bool> DeleteMatchData(MatchDataDto matchData);


	//public Task<List<EventSchedule>> GetEventSchedules();

	//public Task<bool> AddEventSchedule(EventSchedule eventSchedule);



	//public Task<DataToSend> GetDataToSend();

	//public Task<List<KnownDevice>> GetMostRecentFromDevice();

	//public Task<List<DomainError>> GetDomainErrors();



	public Task<string?> GetLastScout();

	public Task<bool> SetLastScout(string scoutName);

}