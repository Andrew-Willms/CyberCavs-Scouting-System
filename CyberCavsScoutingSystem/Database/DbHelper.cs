using System.Data.SQLite;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;

namespace Database;



public class DbHelper {

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

	public List<GameSpec> GetGameSpecs() {
		throw new NotImplementedException();
	}

	public bool AddGameSpec() {
		throw new NotImplementedException();
	}

	public List<MatchData> GetMatchData() {
		throw new NotImplementedException();
	}

	public bool AddMatchData(MatchData matchData) {
		throw new NotImplementedException();
	}

	public bool AddMatchData(List<MatchData> matchData) {
		throw new NotImplementedException();
	}

	public List<(string uuid, DateTime timeStamp)> GetMostRecentFromDevice() {
		throw new NotImplementedException();
	}

	public List<DomainError> GetDomainErrors() {
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