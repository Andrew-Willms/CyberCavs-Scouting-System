using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class SavedMatchesPage : ContentPage, INotifyPropertyChanged {

	public static string Route => "SavedMatches";

	private IDataStore DataStore { get; }
	private IErrorPresenter ErrorPresenter { get; }

	private bool IsActuallyRefreshing;
	public bool IsRefreshing {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(IsRefreshing));
		}
	}

	public string? GetMatchesError {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(GetMatchesError));
		}
	}

	public ObservableCollection<MatchDataDto> SavedMatches { get; } = [];


	public SavedMatchesPage(IDataStore dataStore, IErrorPresenter errorPresenter) {
		
		DataStore = dataStore;
		ErrorPresenter = errorPresenter;

		BindingContext = this;
		InitializeComponent();
	}



	private async Task Refresh() {

		if (IsActuallyRefreshing) {
			return;
		}

		MainThread.BeginInvokeOnMainThread(() => { IsRefreshing = true; });
		IsActuallyRefreshing = true;

		SavedMatches.Clear();

		GetMatchDataResult getMatchDataResult = await DataStore.GetMatchData();

		getMatchDataResult.Switch(
			matchData => {
				foreach (MatchDataDto match in matchData) {
					SavedMatches.Add(match);
				}
			},
			exception => GetMatchesError = 
				$"Could not fetch matches. Exception raised of type '{exception.GetType()}' with message:\r\n\r\n{exception.Message}",
			matchDataDeserializationError => GetMatchesError = 
				$"Could not fetch matches. Deserialization error. Serialized data:\r\n\r\n{matchDataDeserializationError.SerializedMatchData}",
			invalidEditIdsError => GetMatchesError = "Could not fetch matches. There are invalid edit IDs."
		);

		IsActuallyRefreshing = false;
		MainThread.BeginInvokeOnMainThread(() => { IsRefreshing = false; });
	}

	private async Task<bool> DeleteMatch(MatchDataDto matchData) {

		return await DataStore.DeleteMatchData(matchData);
	}



	// ReSharper disable once AsyncVoidEventHandlerMethod, async void needed for navigation
	private async void SavedMatchesPage_OnLoaded(object? sender, EventArgs e) {
		await Refresh();
	}

	// ReSharper disable once AsyncVoidEventHandlerMethod, async void needed for navigation
	private async void SavedMatchesView_OnRefreshing(object? sender, EventArgs e) {
		await Refresh();
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void ViewMatch_OnClicked(object? sender, EventArgs e) {

		Button button = sender as Button ?? throw new ArgumentException("sender not valid");
		MatchDataDto matchData = button.BindingContext as MatchDataDto ?? throw new ArgumentException("sender not valid");

		Dictionary<string, object> parameters = new() {
			{ MatchQrCodePage.MatchDataNavigationParameterName, matchData },
#pragma warning disable CS8974 // Converting method group to non-delegate type
			{ MatchQrCodePage.MatchDeleterNavigationParameterName, DeleteMatch }
#pragma warning restore CS8974 // Converting method group to non-delegate type
		};

		await Shell.Current.GoToAsync(MatchQrCodePage.RouteFromSavedMatchesPage, parameters);
	}

	// ReSharper disable once AsyncVoidEventHandlerMethod, async void needed for navigation
	private async void SavedMatchesPage_OnNavigatedTo(object? sender, NavigatedToEventArgs e) {
		await Refresh();
	}

	private async void DeleteAllButton_OnClicked(object? sender, EventArgs e) {

		bool success = await DataStore.DeleteAllMatchData();

		if (success) {
			await Refresh();
			return;
		}

		ErrorPresenter.DisplayError("Failed", "Could not delete all matches.");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}