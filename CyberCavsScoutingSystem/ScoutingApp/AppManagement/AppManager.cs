using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using UtilitiesLibrary.Results;
using static ScoutingApp.IGameSpecRetrievalResult;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace ScoutingApp.AppManagement;



public interface IAppManager : INotifyPropertyChanged {

	public MatchDataCollector ActiveMatchData { get; }
	public string Scout { get; set; }
	public string EventCode { get; set; }

	public Task ApplicationStartup();

	public Task SaveMatchData();

	public Task DiscardAndStartNewMatch();

	public Event OnMatchStarted { get; }

	public Event OnNewData { get; }

	public IDataStore DataStore { get; }

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

	public string EventCode { get; set; } = string.Empty;

	public EventSchedule? EventSchedule { get; set; }

	public Event OnMatchStarted { get; } = new();

	public Event OnNewData { get; } = new();

	private static IErrorPresenter ErrorPresenter => ServiceHelper.GetService<IErrorPresenter>();

	public IDataStore DataStore => ServiceHelper.GetService<IDataStore>();



	public async Task ApplicationStartup() {

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

#if ANDROID
		string deviceId = Android.Provider.Settings.Secure.GetString(
			Platform.CurrentActivity!.ContentResolver, 
			Android.Provider.Settings.Secure.AndroidId)!;
#elif IOS
		string deviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString();
#else
		string deviceId = null as string ?? throw new NotSupportedException();
#endif

		ErrorContext errorContext = new() {
			DeviceId = deviceId,
			DeviceName = DeviceInfo.Current.Name,
			Scout = Scout
		};

		MatchData? matchData = MatchData.FromDataCollector(errorContext, ActiveMatchData, EventCode, EventSchedule, Scout);

		if (matchData is null) {
			throw new NotImplementedException();
		}

		bool success = await DataStore.AddMatchData(matchData);

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



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}