using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui;
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

	public bool AnyMatches => SavedMatches.Any();



	public SavedMatchesPage(IDataStore dataStore, IErrorPresenter errorPresenter) {

		DataStore = dataStore;
		ErrorPresenter = errorPresenter;

		SavedMatches.CollectionChanged += (sender, eventArgs) => {
			OnPropertyChanged(nameof(AnyMatches));
		};

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
				$"The Matches could not be loaded. An exception was raised of type '{exception.GetType()}' with the message:\r\n\r\n{exception.Message}",
			matchDataDeserializationError => {
				GetMatchesError = matchDataDeserializationError switch {
					CouldNotParseValuesError couldNotParseValuesError => 
						$"The matches could not be loaded because of a deserialization error.\r\n\r\n" +
						$"The following values could not be parsed:" +
						$"{string.Join(string.Empty, couldNotParseValuesError.CoreValueErrors.Select(x => 
							$"\r\n  - Column {x.ColumnIndex} with the value '{x.Text}' could not be parsed as the type '{x.ExpectedType.Name}'")
						)}" +
						$"{string.Join(string.Empty, couldNotParseValuesError.DataFieldErrors.Select(x =>
							$"\r\n  - The value '{x.Text}' could not be parsed for the data field '{x.DataField.Name}'")
						)}\r\n\r\n" +
						$"Serialized data:\r\n\r\n{matchDataDeserializationError.SerializedMatchData}",
					WrongNumberOfCsvColumnsError wrongNumberOfCsvColumnsError =>
						$"The matches could not be loaded because of a deserialization error.\r\n\r\n" +
						$"{wrongNumberOfCsvColumnsError.ExpectedColumnCount} CSV columns were expected and " +
						$"{wrongNumberOfCsvColumnsError.Columns.Count} were found.\r\n\r\n" +
						$"Serialized data:\r\n\r\n{matchDataDeserializationError.SerializedMatchData}",
					_ => throw new ArgumentOutOfRangeException(nameof(matchDataDeserializationError))
				};
			},
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

		int matchCount = SavedMatches.Count;

		string confirmPrompt = matchCount == 1
			? "Type '1' and tap 'OK' if you want to delete the 1 saved match."
			: $"Type '{matchCount}' and tap 'OK' if you want to delete all saved {matchCount} matches.";

		string result = await DisplayPromptAsync(
			"Are you sure you want to delete all matches?", confirmPrompt, maxLength: 20, keyboard: Keyboard.Numeric);

		if (result != matchCount.ToString()) {
			return;
		}

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