using System.Data.SQLite;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;

namespace Database;



public interface IDataStore {

	public Task<List<GameSpec>> GetGameSpecs();

	public Task<bool> AddGameSpec();

	public Task<List<MatchData>> GetMatchData();

	public Task <bool> AddMatchData(MatchData matchData);

	public Task<bool> AddMatchData(List<MatchData> matchData);

	public Task<List<EventSchedule>> GetEventSchedules();

	public Task<bool> AddEventSchedule(EventSchedule eventSchedule);

	public Task<List<DeviceSynchronization>> GetMostRecentFromDevice();

	public Task<List<DomainError>> GetDomainErrors();

}



public record struct DeviceSynchronization {

	public required string DeviceId { get; init; }

	public required int LatestDataRecordId { get; init; }

	public required int HashOfPrevious { get; init; }

}

public class DataRecord {

	public required int Id { get; init; }

	public required string TableName { get; init; }

	public required int ForeignKey { get; init; }

	public required DateTime TimeCreated { get; init; }

	public required int HashOfPrevious { get; init; }

	public override int GetHashCode() {
		return HashCode.Combine(Id, TableName, ForeignKey, TimeCreated, HashOfPrevious);
	}

}



public class SqliteDataStore : IDataStore {

	public void EnsureCreatedAndConnectToDb(string filePath) {

	}

	public void CreateDb(string filePath) {

		const string connectionString = "Data Source=:memory:";
		const string commandText = "SELECT SQLITE_VERSION()";

		using SQLiteConnection con = new(connectionString);
		con.Open();

		using SQLiteCommand command = new(commandText, con);
		string? version = command.ExecuteScalar()!.ToString();

		Console.WriteLine($"SQLite version: {version}");
	}

	public Task<List<GameSpec>> GetGameSpecs() {
		throw new NotImplementedException();
	}

	public Task<bool> AddGameSpec() {
		throw new NotImplementedException();
	}

	public Task<List<MatchData>> GetMatchData() {
		throw new NotImplementedException();
	}

	public Task<bool> AddMatchData(MatchData matchData) {
		throw new NotImplementedException();
	}

	public Task<bool> AddMatchData(List<MatchData> matchData) {
		throw new NotImplementedException();
	}

	public Task<List<EventSchedule>> GetEventSchedules() {
		throw new NotImplementedException();
	}

	public Task<bool> AddEventSchedule(EventSchedule eventSchedule) {
		throw new NotImplementedException();
	}

	public Task<List<DeviceSynchronization>> GetMostRecentFromDevice() {
		throw new NotImplementedException();
	}

	public Task<List<DomainError>> GetDomainErrors() {
		throw new NotImplementedException();
	}

}


// todo: enable the game maker to define migrations from one version of a game to another

// errors column
// error resolutions column
// concurrent, conflicting, independent

// Known Devices
// ID (?), device name (string), record number (int)

// Match Data (per game)
// PK (?), record number (int), based on (?), previous (?), game fields...

// Event Schedules

// Games
// 