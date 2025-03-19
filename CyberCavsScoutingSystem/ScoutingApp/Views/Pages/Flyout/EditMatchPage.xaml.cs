using System;
using System.ComponentModel;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
public partial class EditMatchPage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/Edit";
	public static string RouteFromSavedMatchesPage => "/Edit";

	public const string MatchDataNavigationParameterName = nameof(MatchDataDto);

	public MatchDataDto SavedMatch {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(SavedMatch));
		}
	} = null!;



	public EditMatchPage() {

		BindingContext = this;
		InitializeComponent();
	}


	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void SaveButton_OnClicked(object? sender, EventArgs e) {
		//await MatchDeleter(SavedMatch);
		await Shell.Current.GoToAsync("..");
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void CancelButton_OnClicked(object? sender, EventArgs e) {
		await Shell.Current.GoToAsync($"../{MatchQrCodePage.RouteFromSavedMatchesPage}");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}