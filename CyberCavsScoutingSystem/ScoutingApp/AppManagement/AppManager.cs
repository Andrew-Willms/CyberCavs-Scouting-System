using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using CCSSDomain.MatchData;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
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

	public Task<string> GetLastScout();

	public Task<bool> SetLastScout(string scoutName);

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
			DataStore.SetLastScout(value);

			var result = AsyncContext.Run(() => MyAsyncMethod());


			OnPropertyChanged(nameof(Scout));
		}
	} = string.Empty;

	public string? ScoutError {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(Scout));
		}
	}

	public string EventCode { get; set; } = string.Empty;

	public EventSchedule? EventSchedule { get; set; }

	public Event OnMatchStarted { get; } = new();

	public Event OnNewData { get; } = new();

	private static IErrorPresenter ErrorPresenter => ServiceHelper.GetService<IErrorPresenter>();

	public IDataStore DataStore => ServiceHelper.GetService<IDataStore>();



	public async Task ApplicationStartup() {

		Scout = await DataStore.GetLastScout();

		await StartNewMatch();

		await GetLastScout();
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



	public Task<List<MatchData>> GetMatchData() {
		throw new NotImplementedException();
	}


	public async Task<string> GetLastScout() {

		return await DataStore.GetLastScout();
	}

	public async Task<bool> SetLastScout(string scoutName) {
		return await DataStore.SetLastScout(scoutName);
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}