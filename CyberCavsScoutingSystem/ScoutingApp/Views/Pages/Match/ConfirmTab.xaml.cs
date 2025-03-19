using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CCSSDomain.DataCollectors;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages.Flyout;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;

namespace ScoutingApp.Views.Pages.Match; 



public partial class ConfirmTab : ContentPage, INotifyPropertyChanged {

	public static string Route => "Confirm";

	private IAppManager AppManager { get; }

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

			if (AppManager.ActiveMatchData.MatchType == Optional.NoValue) {
				errors.Add("MatchType has not been set.");
			}

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



	public ConfirmTab(IAppManager appManager, IErrorPresenter errorPresenter) {

		AppManager = appManager;
		ErrorPresenter = errorPresenter;

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

		await Shell.Current.GoToAsync($"//{SavedMatchesPage.Route}");
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
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}