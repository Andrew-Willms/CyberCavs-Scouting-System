using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using QRCoder;
using ScoutingApp.Views.Pages.Flyout;
using UtilitiesLibrary.Results;
using static ScoutingApp.IGameSpecRetrievalResult;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace ScoutingApp.AppManagement;



public interface IAppManager : INotifyPropertyChanged {

	public MatchDataCollector ActiveMatchData { get; }
	public string Scout { get; set; }
	public string Event { get; set; }

	public Task ApplicationStartup();

	public Task SaveMatchData();

	public Task DiscardAndStartNewMatch();

	public Event OnMatchStarted { get; }

}



public class AppManager : IAppManager, INotifyPropertyChanged {
	public MatchDataCollector ActiveMatchData {
		get;
		private set {
			field = value;
			OnPropertyChanged(nameof(ActiveMatchData));
		}
	} = null!;

	public string Scout {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(Scout));
			Task _ = WriteScoutToDisk();
		}
	} = string.Empty;

	public string Event { get; set; } = string.Empty;

	public Event OnMatchStarted { get; } = new();

	private static IErrorPresenter ErrorPresenter => ServiceHelper.GetService<IErrorPresenter>();



	public async Task ApplicationStartup() {

		EnsureMatchDataDirectoryExists();

		await StartNewMatch();

		await ReadScoutFromDisk();
	}

	private async Task StartNewMatch() {

		IGameSpecRetrievalResult result = await App.GetGameSpec();

		while (result is Loading) {

			await Task.Delay(100);
			result = await App.GetGameSpec();
		}

		if (result is Error error) {

			ErrorPresenter.DisplayError("Error ", error.Message);
		}

		GameSpec gameSpec = (result as IResult<GameSpec>.Success)?.Value ?? throw new UnreachableException();

		ActiveMatchData = new(gameSpec);

		OnMatchStarted.Invoke();
	}

	public async Task SaveMatchData() {

		string csvData = ActiveMatchData.ConvertDataToCsv(Scout, Event);
		string path = Path.Combine(SavedMatchesPage.MatchFileDirectoryPath, $"{DateTime.Now:yyyy-MM-dd HH.mm.ss}{SavedMatchesPage.FileExtension}");

		try {
			await File.WriteAllTextAsync(path, csvData);

		} catch {
			ErrorPresenter.DisplayError(
				"Match Data Could Not Be Saved", 
				"A file could not be created to store the scouting data.");
		}

		await StartNewMatch();
	}

	public async Task DiscardAndStartNewMatch() {

		bool discard = await Shell.Current.DisplayAlert(
			"Discard Current Match?",
			"Do you want to discard the current match and start a new one? Doing so will delete all data entered in this match",
			"Discard and start new match.",
			"Continue with current match.");

		if (!discard) {
			return;
		}

		await StartNewMatch();
	}

	private async Task ReadScoutFromDisk() {

		string scoutFilePath = Path.Combine(FileSystem.Current.CacheDirectory, nameof(Scout));

		try {

			if (!File.Exists(scoutFilePath)) {
				await File.WriteAllTextAsync(scoutFilePath, "");
			}

			Scout = await File.ReadAllTextAsync(scoutFilePath);
		} catch { /*ignored*/ }
	}

	private async Task WriteScoutToDisk() {

		string scoutFilePath = Path.Combine(FileSystem.Current.CacheDirectory, nameof(Scout));

		await File.WriteAllTextAsync(scoutFilePath, Scout);
	}

	private static void EnsureMatchDataDirectoryExists() {

		if (Directory.Exists(SavedMatchesPage.MatchFileDirectoryPath)) {
			return;
		}

		try {
			Directory.CreateDirectory(SavedMatchesPage.MatchFileDirectoryPath);

		} catch {
			ErrorPresenter.DisplayError(
				"Match Data Directory Could Not Be Created", 
				"For unknown reasons the directory for holding scouting data could not be created. " +
				"Because of this match data will not be able to be saved. " +
				"Contact the creators of the CCSS to alert them of this problem.");
		}
	}


	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}