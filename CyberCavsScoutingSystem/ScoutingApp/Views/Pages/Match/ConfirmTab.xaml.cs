using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CCSSDomain.DataCollectors;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages.Flyout;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.Views.Pages.Match; 



public partial class ConfirmTab : ContentPage, INotifyPropertyChanged {

	public static string Route => "Confirm";

	private IAppManager AppManager { get; }
	private IDataStore DataStore { get; }

	private IErrorPresenter ErrorPresenter { get; }

	public ObservableCollection<string> Errors {
		get {

			ObservableCollection<string> errors = [];

			if (AppManager.Scout == "") {
				errors.Add("The scout name has not been set. Go to the Scout page in the flyout menu to set the scout name.");
			}

			if (AppManager.EventCode == "") {
				errors.Add("The event has not been set. Go to the Event page in the flyout menu to set the event.");
			}

			if (AppManager.ActiveMatchData.MatchNumber == Optional.NoValue) {
				errors.Add("The match number has not been set.");
			}

			if (AppManager.ActiveMatchData.ReplayNumber == Optional.NoValue) {
				errors.Add("The replay number has not been set.");
			}

			// todo figure out how to not have the default MatchType overridden
			//if (AppManager.ActiveMatchData.MatchType == Optional.NoValue) {
			//	errors.Add("MatchType has not been set.");
			//}

			if (AppManager.ActiveMatchData.Alliance == Optional.NoValue) {
				errors.Add("An alliance has not been set.");
			}

			if (AppManager.ActiveMatchData.TeamNumber == Optional.NoValue) {
				errors.Add("The team number has not been set.");
			}

			foreach (DataField dataField in AppManager.ActiveMatchData.DataFields) {
				foreach (string error in dataField.Errors) {
					errors.Add(error);
				}
			}

			return errors;
		}
	}

	public bool MatchIstValid => Errors.IsEmpty();



	public ConfirmTab(IAppManager appManager, IErrorPresenter errorPresenter, IDataStore dataStore) {

		AppManager = appManager;
		ErrorPresenter = errorPresenter;
		DataStore = dataStore;

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(""));

		BindingContext = this;
		InitializeComponent();
	}



	private void ConfirmTab_OnNavigatedTo(object? sender, NavigatedToEventArgs e) {
		OnPropertyChanged("");
	}

	private async void SaveButton_OnClicked(object? sender, EventArgs e) {

		bool success = await AppManager.SaveAndStartNewMatch();

		if (!success) {
			ErrorPresenter.DisplayError("Cannot Save Match", "Cannot Save Match"); // todo better error
			return;
		}

		// todo hacky as fuck way to navigate to the most recent match
#if ANDROID
		string deviceId = Android.Provider.Settings.Secure.GetString(
			Platform.CurrentActivity!.ContentResolver,
			Android.Provider.Settings.Secure.AndroidId)!;
#elif IOS
		string deviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.ToString();
#else
		string deviceId = null as string ?? throw new NotSupportedException();
#endif

		List<MatchDataDto>? allData = await DataStore.GetMatchData();
		MatchDataDto? mostRecentMatch = allData?.Where(x => x.DeviceId == deviceId).MaxBy(x => x.RecordId);

		if (mostRecentMatch is null) {
			await Shell.Current.GoToAsync($"//{MatchQrCodePage.Route}");
			return;
		}

		Dictionary<string, object> parameters = new() {
			{ MatchQrCodePage.MatchDataNavigationParameterName, mostRecentMatch },
#pragma warning disable CS8974 // Converting method group to non-delegate type
			{ MatchQrCodePage.MatchDeleterNavigationParameterName, DeleteMatch }
#pragma warning restore CS8974 // Converting method group to non-delegate type
		};

		await Shell.Current.GoToAsync(MatchQrCodePage.RouteFromSavedMatchesPage, parameters);
	}

	private async void DiscardButton_OnClicked(object? sender, EventArgs e) {

		bool discard = await Shell.Current.DisplayAlert(
			"Discard Current Match?",
			"Do you want to discard the current match and start a new one? Doing so will delete all data entered in this match",
			"Discard and start new match.",
			"Continue with current match.");

		if (!discard) {
			return;
		}

		AppManager.DiscardAndStartNewMatch();

		await Shell.Current.GoToAsync($"//{AutoTab.Route}");
	}



	private async Task<bool> DeleteMatch(MatchDataDto matchData) {

		return await DataStore.DeleteMatchData(matchData);
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}