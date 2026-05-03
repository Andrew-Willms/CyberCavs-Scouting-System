using System.Diagnostics;
using System.Drawing;
using Domain.Data;
using Domain.GameSpecification;
using Domain.Serialization;
using Microsoft.Data.Sqlite;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using Success = OneOf.Types.Success;

namespace Database;



public class SqliteDataStore : IDataStore {

	private static class Tables {

		public static class Scout {
			public const string Name = "Scouts";
			public const string NameColumn = "Name";
		}

		public static class KnownDevices {
			public const string Name = "KnownDevices";
			public const string DeviceId = "DeviceId";
			public const string LatestRecordId = "IdOfLatestRecord";
		}

		public static class UnifiedRecords {
			public const string Name = "UnifiedRecords";
			public const string DeviceId = "DeviceId";
			public const string RecordId = "RecordId";
			public const string TableNameColumn = "TableName";
			public const string TimeCreated = "TimeCreated";
		}

		public static class MatchData {
			public const string Name = "MatchData";
			public const string DeviceId = "DeviceId";
			public const string RecordId = "RecordId";
			public const string Data = "Data";
			public const string OriginalDeviceId = "OriginalDeviceId";
			public const string OriginalRecordId = "OriginalRecordId";
		}

	}

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

		} catch {
			return false;
		}

		SqliteCommand createScoutTable = new(
			$"""
			 CREATE TABLE IF NOT EXISTS "{Tables.Scout.Name}" (
			 	"{Tables.Scout.NameColumn}" TEXT NOT NULL
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
			 CREATE TABLE IF NOT EXISTS "{Tables.KnownDevices.Name}" (
			 	"{Tables.KnownDevices.DeviceId}" TEXT NOT NULL PRIMARY KEY,
			 	"{Tables.KnownDevices.LatestRecordId}" INTEGER NOT NULL
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
			CREATE TABLE IF NOT EXISTS '{Tables.UnifiedRecords.Name}' (
				"{Tables.UnifiedRecords.DeviceId}" TEXT NOT NULL,
				"{Tables.UnifiedRecords.RecordId}" INTEGER NOT NULL,
				"{Tables.UnifiedRecords.TableNameColumn}" TEXT NOT NULL,
				"{Tables.UnifiedRecords.TimeCreated}" TEXT NOT NULL,
				PRIMARY KEY ("{Tables.UnifiedRecords.DeviceId}", "{Tables.UnifiedRecords.RecordId}")
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
			 CREATE TABLE IF NOT EXISTS "{Tables.MatchData.Name}" (
			 	"{Tables.MatchData.DeviceId}" TEXT NOT NULL,
			 	"{Tables.MatchData.RecordId}" INTEGER NOT NULL,
			 	"{Tables.MatchData.Data}" TEXT NOT NULL,
			 	"{Tables.MatchData.OriginalDeviceId}" TEXT,
			 	"{Tables.MatchData.OriginalRecordId}" INTEGER,
			 	PRIMARY KEY ("{Tables.MatchData.DeviceId}", "{Tables.MatchData.RecordId}"),
			 	FOREIGN KEY ("{Tables.MatchData.DeviceId}", "{Tables.MatchData.RecordId}")
			 		REFERENCES "{Tables.UnifiedRecords.Name}" ("{Tables.UnifiedRecords.DeviceId}","{Tables.UnifiedRecords.RecordId}")
			 			ON UPDATE RESTRICT
			 			ON DELETE RESTRICT
			 		DEFERRABLE INITIALLY DEFERRED,
			    FOREIGN KEY ("{Tables.MatchData.OriginalDeviceId}", "{Tables.MatchData.OriginalRecordId}")
			 		REFERENCES "{Tables.MatchData.Name}" ("{Tables.MatchData.DeviceId}","{Tables.MatchData.RecordId}")
			 			ON UPDATE RESTRICT
			 			ON DELETE CASCADE
			 );
			 
			 CREATE TRIGGER IF NOT EXISTS "{Tables.MatchData.Name}_DeleteParent"
			 AFTER DELETE ON "{Tables.MatchData.Name}"
			 FOR EACH ROW
			 WHEN OLD."{Tables.MatchData.OriginalDeviceId}" IS NOT NULL AND OLD."{Tables.MatchData.OriginalRecordId}" IS NOT NULL
			 BEGIN
			     DELETE FROM "{Tables.MatchData.Name}"
			     WHERE "{Tables.MatchData.DeviceId}" = OLD."{Tables.MatchData.OriginalDeviceId}"
			       AND "{Tables.MatchData.RecordId}" = OLD."{Tables.MatchData.OriginalRecordId}";
			 END;
			 """,
			Connection);

		// TODO: fix this major issue:
		// when match data that has been edited has been shared it only shares the matchDataDto for the edited version and not the original version
		// this means that the foreign key constraint on the scanning device always fails because only the edited version is shared and the
		// original record isn't transferred.

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
			name: "Rebuilt",
			year: 2026,
			description: "",
			version: new(1, 0, 0),
			robotsPerAlliance: 3u,
			alliancesPerMatch: 2u,
			alliances: new List<AllianceColor> {
				new() { Color = Color.Red, Name = "Red Alliance" },
				new() { Color = Color.Blue, Name = "Blue Alliance" }
			}.ToReadOnly(),
			dataFields: new List<DataFieldSpec> {
				new SelectionDataFieldSpec {
					Name = "Outpost",
					Options = new List<string> { "Yes - Early", "Yes - Late", "No" }.ToReadOnly(),
					InitialValue = "No",
					RequiresValue = true
				},
				new SelectionDataFieldSpec {
					Name = "Depot",
					Options = new List<string> { "Yes - Early", "Yes - Late", "No" }.ToReadOnly(),
					InitialValue = "No",
					RequiresValue = true
				},
				new IntegerDataFieldSpec { Name = "Mid Trips", InitialValue = 0, MinValue = 0, MaxValue = 15 },
				new BooleanDataFieldSpec { Name = "Scored Preload", InitialValue = false },
				new SelectionDataFieldSpec {
					Name = "Auto Climb",
					Options = new List<string> { "Yes", "Attempted", "No" }.ToReadOnly(),
					InitialValue = "No",
					RequiresValue = true
				},
				new SelectionDataFieldSpec {
					Name = "Primary Role",
					Options = new List<string> { "Passing", "Scoring", "Defending" }.ToReadOnly(),
					InitialValue = "",
					RequiresValue = true
				},
				//new MultiIntegerDataFieldSpec { Name = "Fuel", InitialValue = 0, MinValue = 0, MaxValue = 1000 },
				new BooleanDataFieldSpec { Name = "Passing", InitialValue = false },
				new BooleanDataFieldSpec { Name = "Scoring", InitialValue = false },
				new SelectionDataFieldSpec {
					Name = "Defending",
					Options = new List<string> { "None", "Ramp", "Contact", "Ramp + Contact", "Idle" }.ToReadOnly(),
					InitialValue = "None",
					RequiresValue = true
				},
				new BooleanDataFieldSpec { Name = "Shoveled", InitialValue = false },
				new SelectionDataFieldSpec {
					Name = "Accuracy",
					Options = new List<string> { "<60", "70", "80", "90", "99" }.ToReadOnly(),
					InitialValue = "",
					RequiresValue = false
				},
				new SelectionDataFieldSpec {
					Name = "Aimlessness",
					Options = new List<string> { "0", "25", "50", "75", "100" }.ToReadOnly(),
					InitialValue = "0",
					RequiresValue = false
				},
				new SelectionDataFieldSpec {
					Name = "Effectiveness",
					Options = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }.ToReadOnly(),
					InitialValue = "",
					RequiresValue = true
				},
				new BooleanDataFieldSpec { Name = "Beached", InitialValue = false },
				new SelectionDataFieldSpec {
					Name = "Climb",
					Options = new List<string> { "None", " L1", "L2", "L3" }.ToReadOnly(),
					InitialValue = "None",
					RequiresValue = true
				},
				new SelectionDataFieldSpec {
					Name = "Disconnected",
					Options = new List<string> { "0", "25", "50", "75", "100" }.ToReadOnly(),
					InitialValue = "0",
					RequiresValue = true
				},
				new TextDataFieldSpec {
					Name = "Comments",
					InitialValue = "",
					MustNotBeEmpty = false,
					MustNotBeInitialValue = false
				}
			}.ToReadOnly(),
			setupTabInputs: new List<InputSpec>().ToReadOnly(),
			autoTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Outpost", Label = "Outpost" },
				new() { DataFieldName = "Depot", Label = "Depot" },
				new() { DataFieldName = "Mid Trips", Label = "Mid Trips" },
				new() { DataFieldName = "Scored Preload", Label = "Score Preload?" },
				new() { DataFieldName = "Auto Climb", Label = "L1 Climb?" }
			}.ToReadOnly(),
			teleTabInputs: new List<InputSpec> {
				//new() { DataFieldName = "Fuel", Label = "Fuel Scored" },
				new() { DataFieldName = "Primary Role", Label = "Primary Role" },
				new() { DataFieldName = "Passing", Label = "Passing?" },
				new() { DataFieldName = "Scoring", Label = "Scoring?" },
				new() { DataFieldName = "Defending", Label = "Defense" },
				new() { DataFieldName = "Accuracy", Label = "Accuracy %" },
				new() { DataFieldName = "Aimlessness", Label = "Aimlessness %" },
				new() { DataFieldName = "Effectiveness", Label = "Role Effectiveness" },
				new() { DataFieldName = "Shoveled", Label = "Shoveled Fuel?" },
				new() { DataFieldName = "Beached", Label = "Beached Multiple Times?" }
			}.ToReadOnly(),
			endgameTabInputs: new List<InputSpec> {
				new() { DataFieldName = "Climb", Label = "Climb" },
				new() { DataFieldName = "Disconnected", Label = "Disconnected %" },
				new() { DataFieldName = "Comments", Label = "Extra Comments" }
			}.ToReadOnly());


		if (result is not IResult<GameSpec>.Success success) {
			throw new("Game specification was not successfully produced.");
		}

		return Task.FromResult(new List<GameSpec> {
			success.Value
		});
	}

	public async Task<GetMatchDataResult> GetMatchData() {

		SqliteCommand getMatchDataCommand = new(
			$"SELECT * FROM \"{Tables.MatchData.Name}\";",
			Connection);

		SqliteDataReader reader;
		try {
			reader = await getMatchDataCommand.ExecuteReaderAsync();
		} catch (Exception exception) {
			return exception;
		}

		GameSpec gameSpec = (await GetGameSpecs()).FirstOrDefault() ?? throw new UnreachableException(); // todo

		List<MatchDataDto> allMatchDtos = [];
		while (reader.Read()) {

			string deviceId = reader.GetString(0);
			int recordId = reader.GetInt32(1);
			string serializedMatch = reader.GetString(2);
			string? editOfDeviceId = reader[3] is DBNull ? null : reader.GetString(3);
			int? editOfRecordId = reader[4] is DBNull ? null : reader.GetInt32(4);

			MatchDataDeserializationResult result = MatchDataToCsv.Deserialize(serializedMatch, gameSpec);

			if (result.IsT1) {
				return result.AsT1;
			}

			MatchData data = result.AsT0;

			switch (editOfDeviceId, editOfRecordId) {

				case ({ } originatingDeviceId, { } originalRecordId):
					allMatchDtos.Add(new() {
						MatchData = data,
						DeviceId = deviceId,
						RecordId = recordId,
						EditBasedOn = (originatingDeviceId, originalRecordId)
					});
					break;

				case (null, null):
					allMatchDtos.Add(new() {
						MatchData = data,
						DeviceId = deviceId,
						RecordId = recordId,
						EditBasedOn = null
					});
					break;

				case ({ } originatingDeviceId, null):
					return new InvalidEditIdsError(originatingDeviceId);

				case (null, { } originalRecordId):
					return new InvalidEditIdsError(originalRecordId);
			}
		}

		// Since editing match data isn't implemented yet and there is no conflict resolution implemented editing
		// matches won't work and the below code is moot. Instead, just return the original match data.
		//return allMatchDtos.Where(x => x.EditBasedOn is null).ToList();

		// Identify all the match data that are original (not edits of existing match data).
		// Create an "Edit Chain" for each original match (starting with the original match itself).
		List<List<MatchDataDto>> editChains = allMatchDtos
			.Where(x => x.EditBasedOn is null)
			.Select(x => new List<MatchDataDto> { x })
			.ToList();

		// Iterate over all match data that is an edit of prior match data.
		// Ensure that all edits either directly or transitively (through one or more other edit match data records) point to original match data.
		// The current implementation of this relies on lower degree edits being earlier in the list than higher degree edit.
		// This order is not guaranteed but seems to be working, possibly because no one is actually editing data.
		// A first degree edit is an edit of the original data, a second degree edit is an edit of a first degree edit, etc.
		// This implementation also doesn't work with things like edit trees.

		List<MatchDataDto> unlinkedEditData = allMatchDtos.Where(x => x.EditBasedOn is not null).ToList();
		int lastCountOfUnlinkedEditData = unlinkedEditData.Count;
		while (true) {

			foreach (MatchDataDto editData in unlinkedEditData) {

				List<MatchDataDto>? activeEditChain = editChains.FirstOrDefault(x =>
					x.Count > 0 && // should be guaranteed
					(x.Last().DeviceId, x.Last().RecordId) == editData.EditBasedOn);

				// The active edit chain will be null if the edit data is part of an edit branch that was not chosen.
				activeEditChain?.Add(editData);
			}

			// If all the edit data has a home or if the remaining edit paths are not part of the branch that has been chosen, exit.
			// If the edit history of a match has branched only pick on branch and ignore the edit data from the other branches.
			// Whichever branch is returned by the database first will be chosen.
			if (unlinkedEditData.Count == 0 || lastCountOfUnlinkedEditData == unlinkedEditData.Count) {
				break;
			}
		}

		return editChains.Select(x => x.Last()).ToList();
	}

	public async Task<AddNewMatchDataResult> AddNewMatchData(CreateMatchDataDto matchData) {

		string data = MatchDataToCsv.Serialize(matchData.MatchData).Replace("\'", "\'\'");

		// todo right now it's possible for only one of the two edit columns to be null
		// see if there is a way to restrict it so they both have to be null or not null together

		// it's scuffed that I have to call WITH AS twice but I can't find a workaround
		// CTEs can only be consumed by a singled query.
		SqliteCommand addMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId 
			     FROM "{Tables.UnifiedRecords.Name}"
			     WHERE "{Tables.UnifiedRecords.DeviceId}" = '{matchData.DeviceId}'
			 )
			 INSERT INTO "{Tables.MatchData.Name}" (
			     "{Tables.MatchData.DeviceId}",
			     "{Tables.MatchData.RecordId}",
			     "{Tables.MatchData.Data}",
			     "{Tables.MatchData.OriginalDeviceId}",
			     "{Tables.MatchData.OriginalRecordId}"
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     (SELECT lastId FROM temp) + 1,
			     '{data}',
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.DeviceId}'")},
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.RecordId}'")}
			 );
			 WITH temp AS (
			     SELECT COUNT(*) AS lastId 
			     FROM "{Tables.UnifiedRecords.Name}"
			     WHERE "{Tables.UnifiedRecords.DeviceId}" = '{matchData.DeviceId}'
			 )
			 INSERT INTO "{Tables.UnifiedRecords.Name}" (
			     "{Tables.UnifiedRecords.DeviceId}",
			     "{Tables.UnifiedRecords.RecordId}",
			     "{Tables.UnifiedRecords.TableNameColumn}",
			     "{Tables.UnifiedRecords.TimeCreated}"
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     (SELECT lastId FROM temp) + 1,
			     '{Tables.MatchData.Name}',
			     'TimeCreated'
			 );
			 COMMIT;
			 """,
			Connection);

		try {
			await addMatchDataCommand.ExecuteNonQueryAsync();
		} catch (Exception exception) {
			return exception;
		}

		return new Success();
	}

	public async Task<AddMatchDataFromOtherDeviceResult> AddMatchDataFromOtherDevice(MatchDataDto matchData) {

		string data = MatchDataToCsv.Serialize(matchData.MatchData).Replace("\'", "\'\'");

		// TODO: consider parameterized queries? less room for SQL injections??
		// TODO: strings are wrapped in 'string' but ints shouldn't be????

		// TODO: consider switching the order of the inserts. Not sure if that's strictly better, but it 
		// wouldn't depend on the deferment of the constraints as much.
		SqliteCommand addMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 INSERT INTO "{Tables.MatchData.Name}" (
			     "{Tables.MatchData.DeviceId}",
			     "{Tables.MatchData.RecordId}",
			     "{Tables.MatchData.Data}",
			     "{Tables.MatchData.OriginalDeviceId}",
			     "{Tables.MatchData.OriginalRecordId}"
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     '{matchData.RecordId}',
			     '{data}',
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.DeviceId}'")},
			     {(matchData.EditBasedOn is null ? "NULL" : $"'{matchData.EditBasedOn?.RecordId}'")}
			 );
			 INSERT INTO "{Tables.UnifiedRecords.Name}" (
			     "{Tables.UnifiedRecords.DeviceId}",
			     "{Tables.UnifiedRecords.RecordId}",
			     "{Tables.UnifiedRecords.TableNameColumn}",
			     "{Tables.UnifiedRecords.TimeCreated}"
			 )
			 VALUES (
			     '{matchData.DeviceId}',
			     '{matchData.RecordId}',
			     '{Tables.MatchData.Name}',
			     'TimeCreated'
			 );
			 COMMIT;
			 """,
			Connection);

		try {
			await addMatchDataCommand.ExecuteNonQueryAsync();

		} catch (Exception exception) {

			SqliteCommand rollbackCommand = new("ROLLBACK;", Connection);

			try {
				await rollbackCommand.ExecuteNonQueryAsync();
			} catch (Exception rollbackException) {

				// TODO if a rollback fails consider trying to close and reopen the connection
				// also consider running something like a "PRAGMA integrity_check"
				return new CouldNotRollBackError {
					FirstException = exception,
					RollbackException = rollbackException
				};
			}

			return exception.Message.Contains("UNIQUE") // TODO: check for this error better, this seems jank
				? new DuplicateMatchDataError()
				: exception;
		}

		return new Success();
	}

	public async Task<bool> DeleteMatchData(MatchDataDto matchData) {

		SqliteCommand deleteMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 DELETE FROM "{Tables.MatchData.Name}"
			 WHERE "{Tables.MatchData.DeviceId}" = '{matchData.DeviceId}' AND
			       "{Tables.MatchData.RecordId}" = '{matchData.RecordId}';
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

	public async Task<bool> DeleteAllMatchData() {

		SqliteCommand deleteMatchDataCommand = new(
			$"""
			 BEGIN TRANSACTION;
			 DELETE FROM "{Tables.MatchData.Name}";
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
			$"SELECT \"{Tables.Scout.NameColumn}\" FROM \"{Tables.Scout.Name}\" WHERE ROWID = 1;",
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

		// TODO better SQL sanitization (here and above)
		if (scoutName.Contains('\'')) {
			scoutName = scoutName.Replace("'", "''");
		}

		SqliteCommand command = new() {
			CommandText =
				$"""
				 INSERT OR REPLACE INTO "{Tables.Scout.Name}" (ROWID, "{Tables.Scout.NameColumn}")
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