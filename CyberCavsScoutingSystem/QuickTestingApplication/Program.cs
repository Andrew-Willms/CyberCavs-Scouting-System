using System;
using System.IO;
using System.Threading.Tasks;
using Database;

namespace QuickTestingApplication;



public class Program {

	private static async Task Main(string[] args) {

		SqliteDataStore dataStore = new();

		try {
			await dataStore.ConnectAndEnsureTables("test.db");
		} catch (Exception exception) {
			throw;
		}

		string? lastScout = await dataStore.GetLastScout();
		bool success = await dataStore.SetLastScout("test");

		dataStore.AddNewMatchData();

	}

}