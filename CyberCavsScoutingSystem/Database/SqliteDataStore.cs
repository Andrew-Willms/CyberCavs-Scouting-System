using System.Diagnostics;
using System.Drawing;
using CCSSDomain.Data;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using Microsoft.Data.Sqlite;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace Database;



public class SqliteDataStore : IDataStore {

	private const string ScoutTableName = "Scouts";
	private const string ScoutTableColumn = "Name";

	private const string KnownDevicesTableName = "KnownDevices";
	private const string KnownDevicesIdColumn = "DeviceId";
	private const string KnownDevicesRecordIdColumn = "IdOfLatestRecord";

	private const string UnifiedRecordTableName = "UnifiedRecords";
	private const string UnifiedRecordDeviceColumn = "OriginatingDevice";
	private const string UnifiedRecordIdColumn = "Id";
	private const string UnifiedRecordTableColumn = "TableName";
	private const string UnifiedRecordDateColumn = "TimeCreated";

	private const string MatchDataTableName = "MatchData";
	private const string MatchDataDeviceColumn = "OriginatingDevice";
	private const string MatchDataIdColumn = "Id";
	private const string MatchDataDataColumn = "Data";
	private const string MatchDataEditOfColumn = "EditOfId";

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
			 CREATE TABLE IF NOT EXISTS '{ScoutTableName}' (
			 	'{ScoutTableColumn}' TEXT NOT NULL
			 );
			 """,
			Connection);

		try {
			await createScoutTable.ExecuteNonQueryAsync();
		} catch {
			return false;
		}

		SqliteCommand createKnownDeviceTable = new(
			$"""
			 CREATE TABLE IF NOT EXISTS '{KnownDevicesTableName}' (
			 	'{KnownDevicesIdColumn}' TEXT NOT NULL PRIMARY KEY,
			 	'{KnownDevicesRecordIdColumn}' INTEGER NOT NULL
			 );
			 """,
			Connection);

		try {
			await createKnownDeviceTable.ExecuteNonQueryAsync();
		} catch {
			return false;
		}

		SqliteCommand createUnifiedRecordTable = new(
			$"""
			CREATE TABLE IF NOT EXISTS '{UnifiedRecordTableName}' (
				'{UnifiedRecordDeviceColumn}' TEXT NOT NULL,
				'{UnifiedRecordIdColumn}' INTEGER NOT NULL,
				'{UnifiedRecordTableColumn}' TEXT NOT NULL,
				'{UnifiedRecordDateColumn}' TEXT NOT NULL,
				PRIMARY KEY ('{UnifiedRecordDeviceColumn}', '{UnifiedRecordIdColumn}'),
			);
			""",
			Connection);

		try {
			await createUnifiedRecordTable.ExecuteNonQueryAsync();
		} catch {
			return false;
		}

		SqliteCommand createMatchDataTable = new(
			$"""
			 CREATE TABLE IF NOT EXISTS '{MatchDataTableName}' (
			 	{MatchDataDeviceColumn} TEXT NOT NULL,
			 	{MatchDataIdColumn} INTEGER NOT NULL,
			 	{MatchDataDataColumn} TEXT NOT NULL,
			 	{MatchDataEditOfColumn} TEXT,
			 	PRIMARY KEY ({MatchDataDeviceColumn}, {MatchDataIdColumn}),
			 	FOREIGN KEY ('{MatchDataDeviceColumn}', '{MatchDataIdColumn}')
			 		REFERENCES '{UnifiedRecordTableName}' ('{UnifiedRecordDeviceColumn}','{UnifiedRecordIdColumn}')
			 			ON UPDATE RESTRICT
			 			ON DELETE RESTRICT
			 		DEFERRABLE INITIALLY DEFERRED
			 );
			 """,
			Connection);

		try {
			await createMatchDataTable.ExecuteNonQueryAsync();
		} catch {
			return false;
		}

		SqliteCommand tableCountCommand = new() {
			CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table';",
			Connection = Connection
		};

		try {
			SqliteDataReader reader = await tableCountCommand.ExecuteReaderAsync();
			reader.Read();
			int tableCount = reader.GetInt32(0);
			return tableCount == 4;

		} catch {
			return false;
		}
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
				new SelectionDataFieldSpec {
					Name = "Auto Mobility",
					Options = new List<string> { "Yes", "No", "" }.ToReadOnly(),
					InitialValue = "None",
					RequiresValue = true
				},
				new IntegerDataFieldSpec { Name = "Tele L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new IntegerDataFieldSpec { Name = "Tele Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new SelectionDataFieldSpec {
					Name = "Climb",
					Options = new List<string> { "None", "Deep", "Shallow" }.ToReadOnly(),
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
				new() { DataFieldName = "Auto L1 Coral", Label = "L1 Coral" },
				new() { DataFieldName = "Auto L2 Coral", Label = "L2 Coral" },
				new() { DataFieldName = "Auto L3 Coral", Label = "L3 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "L4 Coral" },
				new() { DataFieldName = "Auto Algae Net", Label = "Algae Net" },
				new() { DataFieldName = "Auto Algae Processor", Label = "Processor Algae" },
				new() { DataFieldName = "Auto Mobility", Label = "Auto Mobility" }
			}.ToReadOnly(),
			teleTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Tele L1 Coral", Label = "L4 Coral" },
				new() { DataFieldName = "Tele L2 Coral", Label = "L4 Coral" },
				new() { DataFieldName = "Tele L3 Coral", Label = "L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "L4 Coral" },
				new() { DataFieldName = "Tele Algae Net", Label = "Algae Net" },
				new() { DataFieldName = "Tele Algae Processor", Label = "Processor Algae" }
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

	public async Task<List<MatchDataDto>?> GetMatchData() {

		SqliteCommand getMatchDataCommand = new(
			$"SELECT (*) FROM '{MatchDataTableName}';",
			Connection);

		SqliteDataReader reader;
		try {
			reader = await getMatchDataCommand.ExecuteReaderAsync();
		} catch {
			return null;
		}

		GameSpec gameSpec = (await GetGameSpecs()).FirstOrDefault() ?? throw new UnreachableException(); // todo

		List<MatchDataDto> allMatchData = [];
		while (reader.Read()) {

			string deviceId = reader.GetString(0);
			int recordId = reader.GetInt32(1);
			string serializedMatch = reader.GetString(2);
			int editOfId = reader.GetInt32(3);

			MatchData? data = MatchDataToCsv.Deserialize(serializedMatch, gameSpec);

			if (data is null) {
				return null; // todo
			}

			allMatchData.Add(new() {
				MatchData = data, 
				DeviceId = deviceId,
				UnifiedRecordId = recordId,
				EditBasedOn = editOfId
			});
		}

		allMatchData.Sort((left, right) => {

			if (left.MatchData.EventCode != right.MatchData.EventCode) {

			}
			// todo
			return 1;
		});

		return allMatchData;
	}

	public async Task<bool> AddNewMatchData(MatchDataDto matchData) {

		string data = MatchDataToCsv.Serialize(matchData.MatchData).Replace("\'", "\'\'");

		// it's scuffed that I have to call WITH AS twice but I can't find a workaround
		// CTEs can only be consumed by a singled query.
		SqliteCommand addMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId FROM '{UnifiedRecordTableName}'
			 )
			 INSERT INTO '{MatchDataTableName}' (
			     '{MatchDataDeviceColumn}',
			     '{MatchDataIdColumn}',
			     '{MatchDataDataColumn}'
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     (SELECT lastId FROM temp) + 1,
			     '{data}'
			 );
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId FROM '{UnifiedRecordTableName}'
			 )
			 INSERT INTO '{UnifiedRecordTableName}' (
			     '{UnifiedRecordDeviceColumn}',
			     '{UnifiedRecordIdColumn}',
			     '{UnifiedRecordTableColumn}',
			     '{UnifiedRecordDateColumn}'
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			 (SELECT lastId FROM temp) + 1,
			     '{MatchDataTableName}',
			     'TimeCreated'
			 );
			 COMMIT;
			 """,
			Connection);

		try {
			await addMatchDataCommand.ExecuteNonQueryAsync();
		} catch {
			return false;
		}

		return true;
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