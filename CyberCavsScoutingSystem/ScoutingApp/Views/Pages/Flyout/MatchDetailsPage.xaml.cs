using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages.Flyout;



[QueryProperty(nameof(SavedMatch), SerializedMatchNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchDetailsPage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{SavedMatchesPage.Route}/Details";
	public static string RouteFromQrCodePage => "/Details";

	public const string SerializedMatchNavigationParameterName = nameof(SerializedMatch);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

	private SerializedMatch _SavedMatch;
	public SerializedMatch SavedMatch {
		get => _SavedMatch;
		set {
			_SavedMatch = value;
			OnPropertyChanged(nameof(SavedMatch));
		}
	}

	public Func<SerializedMatch, Task> MatchDeleter { get; init; } = null!;



	public MatchDetailsPage() {

		BindingContext = this;
		InitializeComponent();
	}



	private async void DeleteButton_OnClicked(object? sender, EventArgs e) {
		await MatchDeleter(SavedMatch);
		await Shell.Current.GoToAsync("..");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}