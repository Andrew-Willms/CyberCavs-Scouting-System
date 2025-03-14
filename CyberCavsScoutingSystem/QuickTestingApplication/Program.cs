using System.IO;
using Database;

namespace QuickTestingApplication;



public class Program {

	private static void Main(string[] args) {

		SqliteDataStore dataStore = new();

		dataStore.ConnectAndEnsureTables("test.db");

		dataStore.GetLastScout();

	}

}