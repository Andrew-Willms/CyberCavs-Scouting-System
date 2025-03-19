using System.Diagnostics;
using System.Drawing;
using CCSSDomain.Data;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using Microsoft.Data.Sqlite;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Database;



public class SqliteDataStore : IDataStore {

	private const string ScoutTableName = "Scouts";
	private const string ScoutTableColumn = "Name";

	private const string KnownDevicesTableName = "KnownDevices";
	private const string KnownDevicesIdColumn = "DeviceId";
	private const string KnownDevicesRecordIdColumn = "IdOfLatestRecord";

	private const string UnifiedRecordTableName = "UnifiedRecords";
	private const string UnifiedRecordDeviceIdColumn = "OriginatingDevice";
	private const string UnifiedRecordRecordIdColumn = "Id";
	private const string UnifiedRecordTableColumn = "TableName";
	private const string UnifiedRecordDateColumn = "TimeCreated";

	private const string MatchDataTableName = "MatchData";
	private const string MatchDataDeviceIdColumn = "OriginatingDevice";
	private const string MatchDataRecordIdColumn = "Id";
	private const string MatchDataDataColumn = "Data";
	private const string MatchDataEditOfDeviceColumn = "EditOfDeviceId";
	private const string MatchDataEditOfRecordColumn = "EditOfRecordId";

	private SqliteConnection Connection = null!;

	public async Task<bool> ConnectAndEnsureTables(string dbPath) {

		try {
			Connection = new($"Data Source={dbPath}");
			Connection.Open();
		} catch {
			return false;
		}

		SqliteCommand test = new() {
			CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table';",
			Connection = Connection
		};

		try {
			SqliteDataReader reader = await test.ExecuteReaderAsync();
			reader.Read();
			int tableCount = reader.GetInt32(0);

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
				'{UnifiedRecordDeviceIdColumn}' TEXT NOT NULL,
				'{UnifiedRecordRecordIdColumn}' INTEGER NOT NULL,
				'{UnifiedRecordTableColumn}' TEXT NOT NULL,
				'{UnifiedRecordDateColumn}' TEXT NOT NULL,
				PRIMARY KEY ('{UnifiedRecordDeviceIdColumn}', '{UnifiedRecordRecordIdColumn}')
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
			 	{MatchDataDeviceIdColumn} TEXT NOT NULL,
			 	{MatchDataRecordIdColumn} INTEGER NOT NULL,
			 	{MatchDataDataColumn} TEXT NOT NULL,
			 	{MatchDataEditOfDeviceColumn} TEXT,
			 	{MatchDataEditOfRecordColumn} INTEGER,
			 	PRIMARY KEY ({MatchDataDeviceIdColumn}, {MatchDataRecordIdColumn}),
			 	FOREIGN KEY ('{MatchDataDeviceIdColumn}', '{MatchDataRecordIdColumn}')
			 		REFERENCES '{UnifiedRecordTableName}' ('{UnifiedRecordDeviceIdColumn}','{UnifiedRecordRecordIdColumn}')
			 			ON UPDATE RESTRICT
			 			ON DELETE RESTRICT
			 		DEFERRABLE INITIALLY DEFERRED,
			    FOREIGN KEY ('{MatchDataEditOfDeviceColumn}', '{MatchDataEditOfRecordColumn}')
			 		REFERENCES '{MatchDataTableName}' ('{MatchDataDeviceIdColumn}','{MatchDataRecordIdColumn}')
			 			ON UPDATE RESTRICT
			 			ON DELETE RESTRICT
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
				new BooleanDataFieldSpec {
					Name = "Auto Mobility",
					InitialValue = false
				},
				new IntegerDataFieldSpec { Name = "Tele L1 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L2 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L3 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele L4 Coral", InitialValue = 0, MinValue = 0, MaxValue = 12 },
				new IntegerDataFieldSpec { Name = "Tele Algae Net", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new IntegerDataFieldSpec { Name = "Tele Algae Processor", InitialValue = 0, MinValue = 0, MaxValue = 255 },
				new SelectionDataFieldSpec {
					Name = "Climb",
					Options = new List<string> { "None", "Deep", "Shallow", "Failed" }.ToReadOnly(),
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
			$"SELECT * FROM '{MatchDataTableName}';",
			Connection);

		SqliteDataReader reader;
		try {
			reader = await getMatchDataCommand.ExecuteReaderAsync();
		} catch {
			return null;
		}

		GameSpec gameSpec = (await GetGameSpecs()).FirstOrDefault() ?? throw new UnreachableException(); // todo

		List<MatchDataDto> allMatchDtos = [];
		while (reader.Read()) {

			string deviceId = reader.GetString(0);
			int recordId = reader.GetInt32(1);
			string serializedMatch = reader.GetString(2);
			string? editOfDeviceId = reader[3] is DBNull ? null : reader.GetString(3);
			int? editOfRecordId = reader[4] is DBNull ? null : reader.GetInt32(4);

			MatchData? data = MatchDataToCsv.Deserialize(serializedMatch, gameSpec);

			if (data is null) {
				return null; // todo
			}

			switch (editOfDeviceId is null, editOfRecordId is null) {

				case (false, false):
					allMatchDtos.Add(new() {
						MatchData = data,
						DeviceId = deviceId,
						RecordId = recordId,
						EditBasedOn = (editOfDeviceId!, (int)editOfRecordId!)
					});
					break;

				case (true, true):
					allMatchDtos.Add(new() {
						MatchData = data,
						DeviceId = deviceId,
						RecordId = recordId,
						EditBasedOn = null
					});
					break;

				default:
					return null; //todo
			}
		}

		List<List<MatchDataDto>> editChains = allMatchDtos
			.Where(x => x.EditBasedOn is null)
			.Select(x => new List<MatchDataDto> { x })
			.ToList();

		foreach (MatchDataDto editData in allMatchDtos.Where(x => x.EditBasedOn is not null)) {

			List<MatchDataDto>? editChain = editChains.FirstOrDefault(x => x.Any(xx => (xx.DeviceId, xx.RecordId) == editData.EditBasedOn));

			if (editChain is null) {
				return null; // todo
			}

			editChain.Insert(0, editData);
		}

		return editChains.Select(x => x.First()).ToList();
	}

	public async Task<bool> AddNewMatchData(CreateMatchDataDto matchData) {

		string data = MatchDataToCsv.Serialize(matchData.MatchData).Replace("\'", "\'\'");

		// todo right now it's possible for only one of the two edit columns to be null
		// see if there is a way to restrict it so they both have to be null or not null together

		// it's scuffed that I have to call WITH AS twice but I can't find a workaround
		// CTEs can only be consumed by a singled query.
		SqliteCommand addMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId FROM '{UnifiedRecordTableName}'
			 )
			 INSERT INTO '{MatchDataTableName}' (
			     '{MatchDataDeviceIdColumn}',
			     '{MatchDataRecordIdColumn}',
			     '{MatchDataDataColumn}',
			     '{MatchDataEditOfDeviceColumn}',
			     '{MatchDataEditOfRecordColumn}'
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     (SELECT lastId FROM temp) + 1,
			     '{data}',
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.DeviceId}'")},
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.RecordId}'")}
			 );
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId FROM '{UnifiedRecordTableName}'
			 )
			 INSERT INTO '{UnifiedRecordTableName}' (
			     '{UnifiedRecordDeviceIdColumn}',
			     '{UnifiedRecordRecordIdColumn}',
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

	public async Task<bool> AddMatchDataFromOtherDevice(MatchDataDto matchData) {

		string data = MatchDataToCsv.Serialize(matchData.MatchData).Replace("\'", "\'\'");

		SqliteCommand addMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 INSERT INTO '{MatchDataTableName}' (
			     '{MatchDataDeviceIdColumn}',
			     '{MatchDataRecordIdColumn}',
			     '{MatchDataDataColumn}',
			     '{MatchDataEditOfDeviceColumn}',
			     '{MatchDataEditOfRecordColumn}'
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     '{matchData.RecordId}',
			     '{data}',
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.DeviceId}'")},
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.RecordId}'")}
			 );
			 INSERT INTO '{UnifiedRecordTableName}' (
			     '{UnifiedRecordDeviceIdColumn}',
			     '{UnifiedRecordRecordIdColumn}',
			     '{UnifiedRecordTableColumn}',
			     '{UnifiedRecordDateColumn}'
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     '{matchData.RecordId}',
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

	public async Task<bool> DeleteMatchData(MatchDataDto matchData) {

		SqliteCommand deleteMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 DELETE FROM {MatchDataTableName}
			 WHERE {MatchDataDeviceIdColumn} = '{matchData.DeviceId}' AND
			       {MatchDataRecordIdColumn} = '{matchData.RecordId}';
			 DELETE FROM '{UnifiedRecordTableName}'
			 WHERE {UnifiedRecordDeviceIdColumn} = '{matchData.DeviceId}' AND
			       {UnifiedRecordRecordIdColumn} = '{matchData.RecordId}';
			 COMMIT;
			 """,
			Connection);

		try {
			await deleteMatchDataCommand.ExecuteNonQueryAsync();

		} catch {
			return false;
		}

		return true;
	}

	public async Task<string?> GetLastScout() {

		SqliteCommand command = new(
			$"SELECT {ScoutTableColumn} FROM '{ScoutTableName}' WHERE ROWID = 1;",
			Connection
		);

		try {
			object? result = await command.ExecuteScalarAsync();

			return result as string;

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