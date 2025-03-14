using System.Diagnostics;
using System.Drawing;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;
using Microsoft.Data.Sqlite;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace Database;



public interface IDataStore {

	public Task<bool> ConnectAndEnsureTables(string dbPath);



	public Task<List<GameSpec>> GetGameSpecs();

	//public Task<bool> AddGameSpec();



	public Task<List<MatchData>> GetMatchData();

	public Task <bool> AddNewMatchData(MatchData matchData);

	public Task<bool> AddMatchData(List<MatchData> matchData);



	//public Task<List<EventSchedule>> GetEventSchedules();

	//public Task<bool> AddEventSchedule(EventSchedule eventSchedule);



	public Task<DataToSend> GetDataToSend();

	public Task<List<DeviceSynchronization>> GetMostRecentFromDevice();

	//public Task<List<DomainError>> GetDomainErrors();



	public Task<string?> GetLastScout();

	public Task<bool> SetLastScout(string scoutName);

}



public readonly record struct DataToSend {

	public required List<GameSpec> GameSpecifications { get; init; }

	public required List<EventSchedule> EventSchedules { get; init; }

	public required List<MatchData> MatchData { get; init; }

}


public readonly record struct DeviceSynchronization {

	public required string DeviceId { get; init; }

	public required int IdOfLatestRecord { get; init; }

}


public class DataRecord {

	public required int Id { get; init; }

	public required string OriginatingDevice { get; init; }

	public required string TableName { get; init; }

	public required int ForeignKey { get; init; }

	public required DateTime TimeCreated { get; init; }

}



public class SqliteDataStore : IDataStore {

	private const string ScoutTableName = "Scouts";
	private const string ScoutTableColumn = "Name";

	private const string UnifiedRecordTableName = "UnifiedRecords";
	private const string UnifiedRecordIdColumn = "Id";
	private const string UnifiedRecordDeviceColumn = "OriginatingDevice";
	private const string UnifiedRecordTableColumn = "TableName";
	private const string UnifiedRecordForeignKeyColumn = "ForeignKey";
	private const string UnifiedRecordDateColumn = "TimeCreated";

	private const string SynchronizationTableName = "DeviceSynchroniation";
	private const string SynchronizationDeviceIdColumn = "DeviceId";
	private const string SynchronizationRecordIdColumn = "IdOfLatestRecord";

	private const string MatchDataTableName = "MatchData";
	private const string MatchDataIdColumn = "Id";
	private const string MatchDataDataColumn = "Data";

	private SqliteConnection Connection = null!;

	public async Task<bool> ConnectAndEnsureTables(string dbPath) {

		try {
			Connection = new($"Data Source={dbPath}");
			Connection.Open();
		} catch {
			return false;
		}

		SqliteCommand createScoutTable = new(
			$"""
			CREATE TABLE IF NOT EXISTS '{UnifiedRecordTableName}' (
				{UnifiedRecordDeviceColumn} TEXT NOT NULL,
				{UnifiedRecordIdColumn} INTEGER NOT NULL,
				{UnifiedRecordTableColumn} TEXT NOT NULL,
				{UnifiedRecordForeignKeyColumn} INTEGER NOT NULL,
				{UnifiedRecordDateColumn} TEXT NOT NULL,
				PRIMARY KEY ({UnifiedRecordDeviceColumn}, {UnifiedRecordIdColumn})
			);
			""",
			Connection);

		int scoutTableResult = await createScoutTable.ExecuteNonQueryAsync();

		SqliteCommand createDeviceSynchronizationTable = new(
			$"""
			CREATE TABLE IF NOT EXISTS '{SynchronizationTableName}' (
				{SynchronizationDeviceIdColumn} TEXT NOT NULL PRIMARY KEY,
				{SynchronizationRecordIdColumn} INTEGER NOT NULL,
			);
			""",
			Connection);

		int deviceSynchronizationTableResult = createDeviceSynchronizationTable.ExecuteNonQuery();

		SqliteCommand checkIfTableExists2 = new() {
			CommandText = "SELECT name FROM sqlite_master WHERE type='table';",
			Connection = Connection
		};

		SqliteCommand createMatchDataTable = new(
			$"""
			 CREATE TABLE IF NOT EXISTS '{MatchDataTableName}' (
			 	{MatchDataIdColumn} INTEGER NOT NULL PRIMARY KEY,
			 	{MatchDataDataColumn} TEXT NOT NULL,
			 	FOREIGN KEY ('{MatchDataIdColumn}')
			 		REFERENCES '{UnifiedRecordTableName}' ('{UnifiedRecordIdColumn}')
			 			ON UPDATE RESTRICT
			 			ON DELETE RESTRICT
			 );
			 """,
			Connection);

		int matchDataTableResult = createMatchDataTable.ExecuteNonQuery();

		SqliteDataReader reader2 = await checkIfTableExists2.ExecuteReaderAsync();

		reader2.Read();
		string testResult = reader2.GetString(0);

		return true;
	}

	public Task<List<GameSpec>> GetGameSpecs() {

		IResult<GameSpec> result = GameSpec.Create(
			name: "ReefScape",
			year: 2025,
			description: "",
			version: new(1, 0, 0),
			robotsPerAlliance: 3u,
			alliancesPerMatch: 2u,
			alliances: new List<AllianceColor> {
				new() { Color = Color.Red, Name = "Red Alliance" },
				new() { Color = Color.Blue, Name = "Blue Alliance" }
			}.ToReadOnly(),
			dataFields: new List<DataFieldSpec> {
				new IntegerDataFieldSpec { Name = "Auto L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Auto L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Auto L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Auto L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Auto Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new IntegerDataFieldSpec { Name = "Auto Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new IntegerDataFieldSpec { Name = "Tele L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new IntegerDataFieldSpec { Name = "Tele Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new SelectionDataFieldSpec {
					Name = "Climb",
					Options = new List<string> { "Deep", "Shallow", "None" }.ToReadOnly(),
					InitialValue = "None", 
					RequiresValue = true
				},
				new SelectionDataFieldSpec {
					Name = "Disconnected",
					Options = new List<string> { "None of match", "Some of match", "Most of match", "All of match" }.ToReadOnly(),
					InitialValue = "None of match",
					RequiresValue = true
				},
				new SelectionDataFieldSpec {
					Name = "Defense",
					Options = new List<string> { "N/A", "1", "2", "3", "4", "5" }.ToReadOnly(),
					InitialValue = "N/A",
					RequiresValue = true
				},
				new TextDataFieldSpec { Name = "Comments", InitialValue = "", MustNotBeEmpty = true, MustNotBeInitialValue = false }
			}.ToReadOnly(),
			setupTabInputs: new List<InputSpec>().ToReadOnly(),
			autoTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" }
			}.ToReadOnly(),
			teleTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" }
			}.ToReadOnly(),
			endgameTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Climb", Label = "Climb" },
				new() { DataFieldName = "Disconnected", Label = "Disconnected" },
				new() { DataFieldName = "Defense", Label = "Defense Effectiveness" },
				new() { DataFieldName = "Comments", Label = "Comments" },
			}.ToReadOnly());


		if (result is not IResult<GameSpec>.Success success) {
			throw new("Game specification was not successfully produced.");
		}

		return Task.FromResult(new List<GameSpec> {
			success.Value
		});
	}

	public Task<List<MatchData>> GetMatchData() {
		throw new NotImplementedException();
	}

	public Task<bool> AddNewMatchData(MatchData matchData) {
		throw new NotImplementedException();
	}

	public Task<bool> AddMatchData(List<MatchData> matchData) {
		throw new NotImplementedException();
	}

	public Task<DataToSend> GetDataToSend() {
		throw new NotImplementedException();
	}

	public Task<List<DeviceSynchronization>> GetMostRecentFromDevice() {
		throw new NotImplementedException();
	}

	public async Task<string?> GetLastScout() {

		SqliteCommand command = new(
			$"SELECT '{ScoutTableColumn}' FROM '{ScoutTableName}' Where ROWID = 1;",
			Connection
		);

		//SqliteCommand command = new(
		//	$"""
		//	SELECT '{ScoutTableColumn}' 
		//	FROM (
		//	    SELECT MAX(ROWID), '{ScoutTableColumn}' AS '{ScoutTableColumn}' 
		//	    FROM '{ScoutTableName}'
		//	);
		//	""",
		//	Connection
		//);

		//SqliteCommand command = new(
		//	$"""
		//	 SELECT '{ScoutTableColumn}'
		//	 FROM '{ScoutTableName}' WHERE ROWID IN(
		//	 	SELECT MAX(ROWID) FROM '{ScoutTableName}'
		//	 );
		//	 """,
		//	Connection
		//);

		try {
			object? result = await command.ExecuteScalarAsync();

			return result is DBNull
				? string.Empty
				: result as string ?? throw new UnreachableException();

		} catch {
			return null;
		}
	}

	public async Task<bool> SetLastScout(string scoutName) {

		if (scoutName.Contains('\'')) {
			scoutName = scoutName.Replace("'", "''");
		}

		SqliteCommand command = new() {
			CommandText =
				$"""
				 INSERT OR REPLACE INTO '{ScoutTableName}' (ROWID, '{ScoutTableColumn}')
				 VALUES (1, '{scoutName}');
				 """,
			Connection = Connection
		};

		try {
			int result = await command.ExecuteNonQueryAsync();
			return result == 1;

		} catch {
			return false;
		}
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