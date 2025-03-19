using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchQrCodePage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/QrCode";
	public static string RouteFromSavedMatchesPage => "/QrCode";

	public const string MatchDataNavigationParameterName = nameof(MatchDataDto);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

	private IErrorPresenter ErrorPresenter { get; }

	public MatchDataDto SavedMatch {
		get;
		set {
			field = value;
			QrCodeContent = MatchDataDtoToCsv.Serialize(value);
			OnPropertyChanged(nameof(SavedMatch));
		}
	} = null!;

	public string QrCodeContent {
		get;
		private set {
			field = value;
			OnPropertyChanged(nameof(QrCodeContent));
		}
	} = null!;

	public Func<MatchDataDto, Task<bool>> MatchDeleter { get; init; } = null!;

	// todo add a button to go straight back to the match page in the auto tab

	public MatchQrCodePage(IErrorPresenter errorPresenter) {

		ErrorPresenter = errorPresenter;

		BindingContext = this;
		InitializeComponent();
	}



	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void DeleteButton_OnClicked(object? sender, EventArgs e) {

		bool success = await MatchDeleter(SavedMatch);

		if (success) {
			await Shell.Current.GoToAsync("..");
			return;
		}

		ErrorPresenter.DisplayError("Failed", "Could not delete match.");
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void EditButton_OnClick(object? sender, EventArgs e) {

		Dictionary<string, object> parameters = new() {
			{ EditMatchPage.MatchDataNavigationParameterName, SavedMatch },
		};

		await Shell.Current.GoToAsync($"../{EditMatchPage.RouteFromSavedMatchesPage}", parameters);
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}