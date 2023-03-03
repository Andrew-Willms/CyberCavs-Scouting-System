using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Pages;



[QueryProperty(nameof(SavedMatch), SerializedMatchNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchQrCodePage : ContentPage, INotifyPropertyChanged {

	public static string Route => $"{QrCodePage.Route}/Details";
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



	public MatchQrCodePage() {

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