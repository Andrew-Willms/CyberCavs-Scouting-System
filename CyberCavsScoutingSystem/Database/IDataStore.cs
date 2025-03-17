using CCSSDomain.Data;
using CCSSDomain.GameSpecification;

namespace Database;



public interface IDataStore {

	public Task<bool> ConnectAndEnsureTables(string dbPath);



	public Task<List<GameSpec>> GetGameSpecs();

	//public Task<bool> AddGameSpec();



	public Task<List<MatchData>> GetMatchData();

	public Task <bool> AddNewMatchData(string deviceId, MatchData matchData);

	public Task<bool> AddMatchDataFromOtherDevice(MatchData matchData);



	//public Task<List<EventSchedule>> GetEventSchedules();

	//public Task<bool> AddEventSchedule(EventSchedule eventSchedule);



	public Task<DataToSend> GetDataToSend();

	public Task<List<KnownDevice>> GetMostRecentFromDevice();

	//public Task<List<DomainError>> GetDomainErrors();



	public Task<string?> GetLastScout();

	public Task<bool> SetLastScout(string scoutName);

}