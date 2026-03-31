using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using OneOf;
using OneOf.Types;

namespace Database;



[GenerateOneOf]
public partial class AddNewMatchDataResult : OneOfBase<Success, Exception>;

[GenerateOneOf]
public partial class GetMatchDataResult : OneOfBase<List<MatchDataDto>, Exception, MatchDataDeserializationError, InvalidEditIdsError>;

public class MatchDataDeserializationError;

public class InvalidEditIdsError;


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