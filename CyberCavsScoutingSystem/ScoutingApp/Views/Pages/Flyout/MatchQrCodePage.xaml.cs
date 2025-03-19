using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CCSSDomain.Serialization;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), MatchDataNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchQrCodePage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/QrCode";
	public static string RouteFromQrCodePage => "/QrCode";

	public const string MatchDataNavigationParameterName = nameof(MatchDataDto);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

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

	public Func<MatchDataDto, Task> MatchDeleter { get; init; } = null!;



	public MatchQrCodePage() {

		BindingContext = this;
		InitializeComponent();
	}



	// ReSharper disable once AsyncVoidMethod, async void needed for navigation
	//private async void DeleteButton_OnClicked(object? sender, EventArgs e) {
	//	await MatchDeleter(SavedMatch);
	//	await Shell.Current.GoToAsync("..");
	//}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}