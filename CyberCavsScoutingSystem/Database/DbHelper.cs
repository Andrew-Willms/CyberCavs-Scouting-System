using System.Data.SQLite;
using System.Drawing;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

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



	public Task<string> GetLastScout();

	public Task<bool> SetLastScout(string scoutName);

}



public readonly record struct DataToSend {

	public required List<GameSpec> GameSpecifications { get; init; }

	public required List<EventSchedule> EventSchedules { get; init; }

	public required List<MatchData> MatchData { get; init; }

}


public readonly record struct DeviceSynchronization {

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


	public SqliteDataStore() {

	}


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

		IResult<GameSpec> result = GameSpec.Create(
			"ReefScape",
			2025,
			"",
			new(1, 0, 0),
			3u,
			2u,
			new List<AllianceColor> {
				new() { Color = Color.Red, Name = "Red Alliance" },
				new() { Color = Color.Blue, Name = "Blue Alliance" }
			}.ToReadOnly(),
			new List<DataFieldSpec> {
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
				new TextDataFieldSpec { Name = "Comments", InitialValue = "", MustNotBeEmpty = true, MustNotBeInitialValue = false }
			}.ToReadOnly(),
			new List<InputSpec> {

			}.ToReadOnly(),
			new List<InputSpec> {
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" },
				new() { DataFieldName = "Auto L4 Coral", Label = "Auto L4 Coral" }
			}.ToReadOnly(),
			new List<InputSpec> {
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" },
				new() { DataFieldName = "Tele L4 Coral", Label = "Tele L4 Coral" }
			}.ToReadOnly(),
			new List<InputSpec> {
				new() { DataFieldName = "Climb", Label = "Climb" },
				new() { DataFieldName = "Disconnected", Label = "Disconnected" },
				new() { DataFieldName = "Comments", Label = "Comments" },
			}.ToReadOnly());


		if (result is not IResult<GameSpec>.Success success) {
			throw new("Game specification was not successfully produced.");
		}

		return Task.FromResult(new List<GameSpec> {
			success.Value
		});
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

	public Task<string> GetLastScout() {
		return Task.FromResult("test");
	}

	public Task<bool> SetLastScout(string scoutName) {
		return Task.FromResult(true);
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