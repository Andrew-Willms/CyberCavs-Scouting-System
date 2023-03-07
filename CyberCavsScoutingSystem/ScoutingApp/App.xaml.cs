using System.IO;
using System.Threading.Tasks;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages;
using UtilitiesLibrary.Results;
using static ScoutingApp.IGameSpecRetrievalResult;

namespace ScoutingApp;



public partial class App : Application {

	private static IGameSpecRetrievalResult GameSpecification { get; set; } = new Loading();



	public App() {

		InitializeComponent();

		ServiceHelper.GetService<IAppManager>().ApplicationStartup();

		MainPage = new LoadingGameSpecPage();
	}



	protected override void OnStart() {

		Task<IGameSpecRetrievalResult> gameSpecTask = GetGameSpec();

		gameSpecTask.ContinueWith(async task => {

			GameSpecification = await task;

			await Dispatcher.DispatchAsync(() => {
				MainPage = GameSpecification is Success ? new AppShell() : new GameSpecCouldNotBeLoadedPage();
			});
		});

		base.OnStart();
	}

	public static async Task<IGameSpecRetrievalResult> GetGameSpec() {

		if (GameSpecification is Loaded) {
			return GameSpecification;
		}

		string fileContents;
		try {
			await using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("ChargedUp.cgs");
			using StreamReader reader = new(fileStream);
			fileContents = await reader.ReadToEndAsync();

		} catch {
			GameSpecification = new FileCouldNotBeOpened();
			return GameSpecification;
		}

		GameSpec? gameSpecification = JsonConvert.DeserializeObject<GameSpec>(fileContents, JsonSettings.JsonSerializerSettings);

		if (gameSpecification is null) {
			GameSpecification = new InvalidFileContents();
			return GameSpecification;
		}

		GameSpecification = new Loaded { Value = gameSpecification };
		return GameSpecification;
	}

}



public interface IGameSpecRetrievalResult : IResult<GameSpec> {

	public class Loaded : Success, IGameSpecRetrievalResult { }

	public class Loading : Error, IGameSpecRetrievalResult { }

	public class FileCouldNotBeOpened : Error, IGameSpecRetrievalResult {

		public override string Message { get; init; } = "Could not open the file containing the GameSpecification.";

	}

	public class InvalidFileContents : Error, IGameSpecRetrievalResult {

		public override string Message { get; init; } = "Could not parse the contents of the file containing the GameSpecification.";

	}

}