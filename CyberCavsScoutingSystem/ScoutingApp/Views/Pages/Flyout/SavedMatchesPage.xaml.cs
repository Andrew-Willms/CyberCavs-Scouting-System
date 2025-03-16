using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CCSSDomain.Data;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout; 



public partial class SavedMatchesPage : ContentPage, INotifyPropertyChanged {

	public static string Route => "SavedMatches";

	private IAppManager AppManager { get; }

	private static readonly Mutex RefreshMutex = new();
	private bool IsActuallyRefreshing;
	public bool IsRefreshing {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(IsRefreshing));
		}
	}

	public ObservableCollection<MatchData> SavedMatches { get; } = [];


	public SavedMatchesPage(IAppManager appManager) {
		
		AppManager = appManager;

		BindingContext = this;
		InitializeComponent();
	}



	private async Task Refresh() {

		await Dispatcher.DispatchAsync(RefreshMutex.WaitOne);

		if (IsActuallyRefreshing) {
			return;
		}

		IsRefreshing = true;
		IsActuallyRefreshing = true;

		SavedMatches.Clear();
		foreach (MatchData match in await AppManager.GetMatchData()) {
			SavedMatches.Add(match);
		}

		IsRefreshing = false;
		IsActuallyRefreshing = false;

		await Dispatcher.DispatchAsync(RefreshMutex.ReleaseMutex);
	}

	private static Task DeleteMatch() {
		throw new NotImplementedException();
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
		MatchData matchData = button.BindingContext as MatchData ?? throw new ArgumentException("sender not valid");

		Dictionary<string, object> parameters = new() {
			{ MatchQrCodePage.MatchDataNavigationParameterName, matchData },
			{ MatchQrCodePage.MatchDeleterNavigationParameterName, DeleteMatch }
		};

		await Shell.Current.GoToAsync(MatchQrCodePage.RouteFromQrCodePage, parameters);
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