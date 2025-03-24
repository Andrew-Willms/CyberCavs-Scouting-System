using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages.Match;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchQrCodePage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/QrCode";
	public static string RouteFromSavedMatchesPage => "/QrCode";

	public const string MatchDataNavigationParameterName = nameof(MatchDataDto);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

	private IAppManager AppManager { get; }
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

	public MatchQrCodePage(IAppManager appManager, IErrorPresenter errorPresenter) {

		AppManager = appManager;
		ErrorPresenter = errorPresenter;

		BindingContext = this;
		InitializeComponent();
	}



	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void DeleteButton_OnClicked(object? sender, EventArgs e) {

		bool success = await MatchDeleter(SavedMatch);

		if (success) {
			await Shell.Current.GoToAsync($"//{SavedMatchesPage.Route}");
			return;
		}

		ErrorPresenter.DisplayError("Failed", "Could not delete match.");
	}

	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	private async void EditButton_OnClick(object? sender, EventArgs e) {

		bool discard = await Shell.Current.DisplayAlert(
			"Discard Current Match and Edit Selected Match",
			"Do you want to discard the current match and start editing the selected One? Doing so will delete all data entered in this match",
			"Discard and start editing.",
			"Continue with current match.");

		if (!discard) {
			return;
		}

		AppManager.DiscardAndStartEditingMatch(SavedMatch);
		await Shell.Current.GoToAsync($"//{SetupTab.Route}");
	}

	private void ReturnToMatch_ButtonClicked(object? sender, EventArgs e) {
		
		//Shell.Current.GoToAsync($"//{SetupTab.Route}");

		Shell.Current.GoToAsync($"../{SetupTab.Route}"); // try this

		//Shell.Current.GoToAsync(".."); // and this
		//Shell.Current.GoToAsync($"//{SetupTab.Route}");
	}

	private void ScanOtherCodes_ButtonClicked(object? sender, EventArgs e) {
		Shell.Current.GoToAsync($"//{QrCodeScanner.Route}");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}