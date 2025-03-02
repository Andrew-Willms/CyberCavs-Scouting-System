using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CCSSDomain.MatchData;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class EditMatchPage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/Details";
	public static string RouteFromSavedMatchesPage => "/Details";

	public const string MatchDataNavigationParameterName = nameof(MatchData);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

	public string MatchTitle => $"{SavedMatch.TeamNumber} - {SavedMatch.Match.Type} Match {SavedMatch.Match.MatchNumber}";

	public MatchData SavedMatch {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(SavedMatch));
			OnPropertyChanged(nameof(MatchTitle));
		}
	} = null!;

	public Func<MatchData, Task> MatchDeleter { get; init; } = null!;



	public EditMatchPage() {
		InitializeComponent();
	}


	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void SaveButton_OnClicked(object? sender, EventArgs e) {
		await MatchDeleter(SavedMatch);
		await Shell.Current.GoToAsync("..");
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void DeleteButton_OnClicked(object? sender, EventArgs e) {
		await MatchDeleter(SavedMatch);
		await Shell.Current.GoToAsync("..");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}