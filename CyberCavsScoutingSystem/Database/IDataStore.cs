﻿using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;

namespace Database;



public interface IDataStore {

	public Task<bool> ConnectAndEnsureTables(string dbPath);



	public Task<List<GameSpec>> GetGameSpecs();

	//public Task<bool> AddGameSpec();



	public Task<List<MatchDataDto>?> GetMatchData();

	public Task <bool> AddNewMatchData(CreateMatchDataDto matchData);

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