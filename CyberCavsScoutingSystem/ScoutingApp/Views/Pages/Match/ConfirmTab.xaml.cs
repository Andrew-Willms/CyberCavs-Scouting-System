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

	public ObservableCollection<string> Errors {
		get {

			ObservableCollection<string> errors = new();

			if (AppManager.Scout == "") {
				errors.Add("The scout name has not been set. Go to the Scout page in the flyout menu to set the scout name.");
			}

			if (AppManager.Event == "") {
				errors.Add("The event has not been set. Go to the Event page in the flyout menu to set the event.");
			}

			if (AppManager.ActiveMatchData.MatchNumber == Optional.NoValue) {
				errors.Add("The match number has not been set.");
			}

			if (AppManager.ActiveMatchData.ReplayNumber == Optional.NoValue) {
				errors.Add("The replay number has not been set.");
			}

			if (AppManager.ActiveMatchData.IsPlayoff == Optional.NoValue) {
				errors.Add("Is playoff has not been set.");
			}

			if (AppManager.ActiveMatchData.Alliance == Optional.NoValue) {
				errors.Add("The Alliance has not been set.");
			}

			if (AppManager.ActiveMatchData.TeamNumber == Optional.NoValue) {
				errors.Add("The team number has not been set.");
			}

			foreach (DataField dataField in AppManager.ActiveMatchData.DataFields) {

				if (dataField is TextDataField { Text:"" } textDataField) {
					errors.Add($"The data field \"{textDataField.Name}\" is empty.");
				}

				if (dataField is not SelectionDataField selectionDataField) {
					continue;
				}

				if (selectionDataField.SelectedOption == Optional.NoValue) {
					errors.Add($"The data field \"{dataField.Name}\" does not have a selected value.");
				}
			}

			return errors;
		}
	}

	public bool MatchIstValid => Errors.IsEmpty();



	public ConfirmTab(IAppManager appManager) {

		AppManager = appManager;

		AppManager.OnMatchStarted.Subscribe(() => OnPropertyChanged(""));

		BindingContext = this;
		InitializeComponent();
	}



	private void ConfirmTab_OnNavigatedTo(object? sender, NavigatedToEventArgs e) {
		OnPropertyChanged("");
	}

	private async void SaveButton_OnClicked(object? sender, EventArgs e) {

		await AppManager.SaveMatchData();

		await Shell.Current.GoToAsync($"//{SavedMatchesPage.Route}");
	}

	private void DiscardButton_OnClicked(object? sender, EventArgs e) {
		AppManager.DiscardAndStartNewMatch();
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}