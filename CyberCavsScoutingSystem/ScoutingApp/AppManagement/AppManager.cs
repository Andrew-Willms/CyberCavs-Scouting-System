using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CCSSDomain.Data;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui.ApplicationModel;
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

	public Task<bool> SaveAndStartNewMatch();

	public void DiscardAndStartNewMatch();

	public Event OnMatchStarted { get; }

	public Event OnNewData { get; }

	public Task<List<MatchDataDto>?> GetMatchData();

}



public class AppManager : IAppManager, INotifyPropertyChanged {

	public GameSpec GameSpecification { get; private set; }

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



	public string EventCode { get; set; }

	public EventSchedule? EventSchedule { get; set; }

	public Event OnMatchStarted { get; } = new();

	public Event OnNewData { get; } = new();

	private static IDataStore DataStore => ServiceHelper.GetService<IDataStore>();



	public AppManager() {

		GameSpecification = null!; // todo fix hack

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

		string test = AppDomain.CurrentDomain.BaseDirectory;

#if ANDROID
		string docsDirectory = Android.App.Application.Context.GetExternalFilesDir(null)!.AbsoluteFile.Path;
#endif

		string appDirectory = FileSystem.Current.AppDataDirectory;
		string dbPath = System.IO.Path.Combine(docsDirectory, "test.db");
		await DataStore.ConnectAndEnsureTables(dbPath);

		string? scoutResult = await DataStore.GetLastScout();
		Scout = scoutResult ?? string.Empty;
		if (scoutResult is null) {
			ScoutError = "Could not load the last scout from the data store.";
		}

		GameSpecification = (await DataStore.GetGameSpecs()).First();

		StartNewMatch();
	}

	private void StartNewMatch() {

		ActiveMatchData = new(GameSpecification);
		OnMatchStarted.Invoke();
	}

	public async Task<bool> SaveAndStartNewMatch() {

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
			return false; // todo better error type
		}

		CreateMatchDataDto createDto = new() {
			MatchData = matchData,
			DeviceId = deviceId,
			EditBasedOn = null
		};

		if (!await DataStore.AddNewMatchData(createDto)) {
			return false; // todo better error type
		}

		StartNewMatch();
		return true;
	}

	public void DiscardAndStartNewMatch() {
		StartNewMatch();
	}



	public async Task<List<MatchDataDto>?> GetMatchData() {
		return await DataStore.GetMatchData();
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}