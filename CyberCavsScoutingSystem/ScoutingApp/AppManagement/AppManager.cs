using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CCSSDomain.Data;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace ScoutingApp.AppManagement;



public interface IAppManager : INotifyPropertyChanged {

	public GameSpec? GameSpecification { get; }

	public MatchDataCollector ActiveMatchData { get; }

	public string Scout { get; set; }
	public string? ScoutError { get; }

	public string EventCode { get; set; }

	public Task ApplicationStartup();

	public Task SaveMatchData();

	public Task DiscardAndStartNewMatch();

	public Event OnMatchStarted { get; }

	public Event OnNewData { get; }

	public Task<List<MatchData>> GetMatchData();

}



public class AppManager : IAppManager, INotifyPropertyChanged {

	public GameSpec? GameSpecification { get; private set; }

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
			if (value == field) {
				return;
			}
			field = value;

			Task.Run(async () => {

				bool success = await DataStore.SetLastScout(value);

				if (success) {
					ScoutError = null;
					return;
				}

				ScoutError = "There was an error saving the scout to the data store.";
			});

			OnPropertyChanged(nameof(Scout));
		}
	} = string.Empty;

	public string? ScoutError {
		get;
		private set {
			field = value;
			OnPropertyChanged(nameof(Scout));
		}
	}



	public string EventCode { get; set; } = string.Empty;

	public EventSchedule? EventSchedule { get; set; }

	public Event OnMatchStarted { get; } = new();

	public Event OnNewData { get; } = new();

	private static IErrorPresenter ErrorPresenter => ServiceHelper.GetService<IErrorPresenter>();

	private static IDataStore DataStore => ServiceHelper.GetService<IDataStore>();



	public AppManager() {

		if (DateTime.Now >= new DateTime(2025, 3, 21) && DateTime.Now <= new DateTime(2025, 3, 22)) {
			EventCode = "Waterloo";

		} else if (DateTime.Now >= new DateTime(2025, 3, 28) && DateTime.Now <= new DateTime(2025, 3, 29)) {
			EventCode = "Windsor";

		} else if (DateTime.Now >= new DateTime(2025, 4, 4) && DateTime.Now <= new DateTime(2025, 4, 5)) {
			EventCode = "DCMP";

		} else {
			EventCode = "Test Event";
		}
	}



	public async Task ApplicationStartup() {

		string appDirectory = FileSystem.Current.AppDataDirectory;
		string dbPath = System.IO.Path.Combine(appDirectory, "test.db");
		await DataStore.ConnectAndEnsureTables(dbPath);

		string? scoutResult = await DataStore.GetLastScout();
		Scout = scoutResult ?? string.Empty;
		if (scoutResult is null) {
			ScoutError = "Could not load the last scout from the data store.";
		}

		GameSpecification = (await DataStore.GetGameSpecs()).First();

		await StartNewMatch();
	}

	private Task StartNewMatch() {

		if (GameSpecification is null) {
			ErrorPresenter.DisplayError("Error ", "Select a Game before starting a match.");
			return Task.CompletedTask;
		}

		ActiveMatchData = new(GameSpecification);

		OnMatchStarted.Invoke();
		return Task.CompletedTask;
	}

	public async Task SaveMatchData() {

#if ANDROID
		string deviceId = Android.Provider.Settings.Secure.GetString(
			Platform.CurrentActivity!.ContentResolver, 
			Android.Provider.Settings.Secure.AndroidId)!;
#elif IOS
		string deviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString();
#else
		string deviceId = null as string ?? throw new NotSupportedException();
#endif

		MatchData? matchData = MatchData.FromDataCollector(ActiveMatchData, EventCode, EventSchedule, Scout);

		if (matchData is null) {
			throw new NotImplementedException();
		}

		bool success = true;//await DataStore.AddMatchData(matchData);

		if (success) {
			await StartNewMatch();
			return;
		}

		throw new NotImplementedException();
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



	public Task<List<MatchData>> GetMatchData() {
		throw new NotImplementedException();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}