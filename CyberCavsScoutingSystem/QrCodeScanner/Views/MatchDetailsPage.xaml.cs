using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace QrCodeScanner.Views;



[QueryProperty(nameof(ScannedMatch), ScannedMatchNavigationParameterName)]
[QueryProperty(nameof(MatchDeleter), MatchDeleterNavigationParameterName)]
public partial class MatchDetailsPage : ContentPage, INotifyPropertyChanged {

	public static string Route => "Details";
	public const string ScannedMatchNavigationParameterName = nameof(ScannedMatch);
	public const string MatchDeleterNavigationParameterName = nameof(MatchDeleter);

    private ScannedMatch _ScannedMatch;
    public ScannedMatch ScannedMatch {
        get => _ScannedMatch;
        set {
            _ScannedMatch = value;
            OnPropertyChanged(nameof(ScannedMatch));
        }
    }

    public Func<ScannedMatch, Task> MatchDeleter { get; init; } = null!;

	public MatchDetailsPage() {

		InitializeComponent();
        BindingContext = this;
    }



	private async void DeleteButton_OnClicked(object? sender, EventArgs e) {
		await MatchDeleter(ScannedMatch);
		await Shell.Current.GoToAsync("..");
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

    private new void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

}