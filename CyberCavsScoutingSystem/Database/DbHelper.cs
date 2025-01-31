using System.Data.SQLite;

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

}