using System.IO;
using System.Threading.Tasks;
using CCSSDomain.GameSpecification;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using UtilitiesLibrary.Results;

namespace DataIngester;



public partial class App : Application {

	private static readonly JsonSerializerSettings JsonSerializerSettings = new() {
		TypeNameHandling = TypeNameHandling.All,
		Formatting = Formatting.Indented
	};

	private static GameSpec? GameSpecification { get; set; }

	public App() {

		InitializeComponent();
		MainPage = new AppShell();
	}

	public static async Task<IResult<GameSpec>> GetGameSpec() {

		if (GameSpecification is not null) {
			return new IResult<GameSpec>.Success { Value = GameSpecification };
		}

		string fileContents;
		try {
			await using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("ChargedUp.cgs");
			using StreamReader reader = new(fileStream);
			fileContents = await reader.ReadToEndAsync();

		} catch {
			return new IResult<GameSpec>.Error("The game specification file could not be read.");
		}

		GameSpec? gameSpecification = JsonConvert.DeserializeObject<GameSpec>(fileContents, JsonSerializerSettings);

		if (gameSpecification is null) {
			return new IResult<GameSpec>.Error("The game specification file could not be read.");
		}

		GameSpecification = gameSpecification;
		return new IResult<GameSpec>.Success { Value = GameSpecification };
	}

}