using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace QrCodeScanner.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	public static string Route => string.Empty;

	private static readonly string MatchFilePath = Path.Combine(
		Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments)!.AbsolutePath,
		"Data.csv");


	private static readonly Mutex RefreshMutex = new();
	private bool IsActuallyRefreshing;

	public bool IsRefreshing {
		get;
		set {
			field = value;
			OnPropertyChanged(nameof(IsRefreshing));
		}
	}

	public ObservableCollection<string> ScannedMatches { get; } = [];



	public MainPage() {

		BindingContext = this;
		InitializeComponent();
	}



	private void MainPage_OnLoaded(object? sender, EventArgs e) {

		Task _ = Refresh();
	}

	private async void ScanNewMatchButton_OnClicked(object? sender, EventArgs e) {

		Dictionary<string, object> parameters = new() {
			{ MatchScannerPage.AddMatchNavigationParameterName, AddMatch }
		};

		await Shell.Current.GoToAsync(MatchScannerPage.Route, parameters);
	}

	private async void ViewMatchDetailsButton_OnClicked(object? sender, EventArgs e) {

		Button button = sender as Button ?? throw new UnreachableException();
		string scannedMatch = button.BindingContext is string match ? match : throw new UnreachableException();

		Dictionary<string, object> parameters = new() {
			{ MatchDetailsPage.ScannedMatchNavigationParameterName, scannedMatch },
			{ MatchDetailsPage.MatchDeleterNavigationParameterName, DeleteMatch }
		};

		await Shell.Current.GoToAsync(MatchDetailsPage.Route, parameters);
	}

	private async void ScannedMatchesView_OnRefreshing(object? sender, EventArgs e) {
		await Refresh();
	}



	private async Task Refresh() {

		await Dispatcher.DispatchAsync(RefreshMutex.WaitOne);

		if (IsActuallyRefreshing) {
			return;
		}

		IsRefreshing = true;
		IsActuallyRefreshing = true;

		await Dispatcher.DispatchAsync(RefreshMutex.ReleaseMutex);

		ScannedMatches.Clear();
		foreach (string matchData in await GetScannedMatches()) {
			ScannedMatches.Add(matchData);
		}

		IsRefreshing = false;
		IsActuallyRefreshing = false;
	}

	private async Task<bool> AddMatch(string matchData) {

		if (ScannedMatches.Contains(matchData)) {
			return false;
		}

		ScannedMatches.Add(matchData);
		await File.WriteAllLinesAsync(MatchFilePath, ScannedMatches);
		return true;
	}

	private async Task DeleteMatch(string match) {

		if (!ScannedMatches.Contains(match)) {

			await Shell.Current.DisplayAlert(
				"Error",
				"The match you are trying to delete was not found in the list of matches." +
				"(Tell Andrew if this happens because it really shouldn't).",
				"Continue on with life I guess.");

			return;
		}

		ScannedMatches.Remove(match);
		await File.WriteAllLinesAsync(MatchFilePath, ScannedMatches);
	}

	private static async Task<string[]> GetScannedMatches() {

		if (!File.Exists(MatchFilePath)) {
			File.Create(MatchFilePath).Close();
			return [];
		}

		string[] lines = (await File.ReadAllTextAsync(MatchFilePath))
			.Split("\n")
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.ToArray();

		string[] orderedLines = lines.OrderByDescending(x => {

			string[] xParts = x.Split('\t');

			if (xParts.Length < 3) {
				return -1; // todo
			}

			bool success = int.TryParse(xParts[2], out int xMatchNumber);

			if (!success) {
				return -1; // todo
			}

			return xMatchNumber;
		}).ToArray();

		return orderedLines;
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}