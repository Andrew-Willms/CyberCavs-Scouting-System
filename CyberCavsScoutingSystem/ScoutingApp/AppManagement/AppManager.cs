using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.DeviceLock;
using CCSSDomain.Data;
using CCSSDomain.DataCollectors;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using UtilitiesLibrary.Optional;
using Event = UtilitiesLibrary.SimpleEvent.Event;

namespace ScoutingApp.AppManagement;



public interface IAppManager : INotifyPropertyChanged {

	public GameSpec? GameSpecification { get; }

	public MatchDataCollector ActiveMatchData { get; }

	public string Scout { get; set; }

	public string EventCode { get; set; }

	public Task ApplicationStartup();

	public Task<bool> SaveAndStartNewMatch();

	public void DiscardAndStartNewMatch();

	public void DiscardAndStartEditingMatch(MatchDataDto matchData);

	public Event OnMatchStarted { get; }

	public Event OnNewData { get; }

	public Task<List<MatchDataDto>?> GetMatchData();

	public Task<string?> GetScoutName();

	public Task<bool> SetScoutName(string name);

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
			field = value;
			OnPropertyChanged(nameof(Scout));
		}
	} = "";


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

//#if ANDROID
//		string directory = Android.App.Application.Context.GetExternalFilesDir(null)!.AbsoluteFile.Path;
//#endif
		string directory = FileSystem.Current.AppDataDirectory;

		string dbPath = System.IO.Path.Combine(directory, "ScoutingApp.db");
		await DataStore.ConnectAndEnsureTables(dbPath);

		GameSpecification = (await DataStore.GetGameSpecs()).First();

		Scout = await GetScoutName() ?? string.Empty; // todo cleanup

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
			EditBasedOn = ActiveMatchData.EditOf is null ? null : (ActiveMatchData.EditOf?.DeviceId!, (int)ActiveMatchData.EditOf?.RecordId!)
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

	public void DiscardAndStartEditingMatch(MatchDataDto matchData) {

		// todo fix this, the check is broken and returns false when it should return true I think
		//if (!matchData.MatchData.GameSpecification.Equals(GameSpecification)) {
		//	throw new NotImplementedException("Figure out how to handle the game specs being different.");
		//}

		ActiveMatchData = new(GameSpecification) {
			MatchNumber = matchData.MatchData.Match.MatchNumber,
			ReplayNumber = matchData.MatchData.Match.ReplayNumber,
			MatchType = matchData.MatchData.Match.Type,
			TeamNumber = matchData.MatchData.TeamNumber,
			Alliance = matchData.MatchData.AllianceIndex,
			EditOf = (matchData.DeviceId, matchData.RecordId)
		};

		for (int i = 0; i < GameSpecification.DataFields.Count; i++) {

			switch (ActiveMatchData.DataFields[i], matchData.MatchData.DataFields[i]) {
				case (BooleanDataField booleanDataField, bool boolValue):
					booleanDataField.Value = boolValue;
					break;
				case (IntegerDataField integerDataField, int intValue):
					integerDataField.Value = intValue;
					break;
				case (SelectionDataField selectionDataField, Optional<string> selection):
					selectionDataField.Value = selection;
					break;
				case (TextDataField textDataField, string text):
					textDataField.Value = text;
					break;
				default:
					throw new NotImplementedException("Figure out how to hand data field type miss-matches");
			}
		}

		OnMatchStarted.Invoke();
	}



	public async Task<List<MatchDataDto>?> GetMatchData() {
		return await DataStore.GetMatchData();
	}

	public async Task<string?> GetScoutName() {
		return await DataStore.GetLastScout();
	}

	public async Task<bool> SetScoutName(string name) {
		return await DataStore.SetLastScout(name);
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}