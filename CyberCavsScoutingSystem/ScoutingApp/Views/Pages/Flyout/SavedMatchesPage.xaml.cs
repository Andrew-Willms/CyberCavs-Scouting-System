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

	private IAppManager AppManager { get; }
	private IDataStore DataStore { get; }

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


	public SavedMatchesPage(IAppManager appManager, IDataStore dataStore) {
		
		AppManager = appManager;
		DataStore = dataStore;

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

		List<MatchDataDto>? matches = await AppManager.GetMatchData();

		if (matches is null) {
			GetMatchesError = "Could not fetch matches.";
			matches = [];

		} else {
			GetMatchesError = null;
		}

		foreach (MatchDataDto match in matches) {
			SavedMatches.Add(match);
		}

		IsActuallyRefreshing = false;
		MainThread.BeginInvokeOnMainThread(() => { IsRefreshing = false; });
	}

	private async Task<bool> DeleteMatch(MatchDataDto matchData) {

		return await DataStore.DeleteMatchData(matchData);
	}


	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void SavedMatchesPage_OnLoaded(object? sender, EventArgs e) {
		await Refresh();
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
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

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void SavedMatchesPage_OnNavigatedTo(object? sender, NavigatedToEventArgs e) {
		await Refresh();
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}