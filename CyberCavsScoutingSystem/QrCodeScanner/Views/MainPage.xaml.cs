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
using UtilitiesLibrary.Collections;

namespace QrCodeScanner.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	public static string Route => string.Empty;

	private static readonly string MatchFileDirectoryPath = Path.Combine(
		Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments)!.AbsolutePath,
		$"CCSS.{nameof(QrCodeScanner)}");

	private const string FileExtension = ".csvl";


	private static readonly Mutex RefreshMutex = new();
	private bool IsActuallyRefreshing;

	private bool _IsRefreshing;
	public bool IsRefreshing {
		get => _IsRefreshing;
		set {
			_IsRefreshing = value;
			OnPropertyChanged(nameof(IsRefreshing));
		}
	}

	public ObservableCollection<ScannedMatch> ScannedMatches { get; } = new();



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
		ScannedMatch scannedMatch = button.BindingContext is ScannedMatch match ? match : throw new UnreachableException();

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
		foreach (ScannedMatch match in await GetScannedMatchesFromFiles()) {
			ScannedMatches.Add(match);
		}

		IsRefreshing = false;
		IsActuallyRefreshing = false;
	}

	private async Task AddMatch(ScannedMatch match) {

		if (ScannedMatches.Contains(match)) {

			await Shell.Current.DisplayAlert(
				"Error",
				"The match you are trying to add is already in the list of matches.",
				"Continue on with life I guess.");

			return;
		}

		ScannedMatches.Add(match);
		await SaveMatchToFile(match);
	}

	private async Task DeleteMatch(ScannedMatch match) {

		if (!ScannedMatches.Contains(match)) {

			await Shell.Current.DisplayAlert(
				"Error",
				"The match you are trying to delete was not found in the list of matches." +
				"(Tell Andrew if this happens because it really shouldn't).",
				"Continue on with life I guess.");

			return;
		}

		ScannedMatches.Remove(match);

		string[] fileNames = Directory.GetFiles(MatchFileDirectoryPath, $"*{FileExtension}", SearchOption.TopDirectoryOnly);

		foreach (string filePath in fileNames) {

			if (match.Content == await File.ReadAllTextAsync(filePath)) {
				File.Delete(filePath);
			}
		}
	}

	private static async Task<ScannedMatch[]> GetScannedMatchesFromFiles() {

		Directory.CreateDirectory(MatchFileDirectoryPath);

		string[] filePaths = Directory.GetFiles(MatchFileDirectoryPath, $"*{FileExtension}", SearchOption.TopDirectoryOnly);

		Task<string>[] fileContents = filePaths.Select(fileName => File.ReadAllTextAsync(fileName)).ToArray();
		string[] fileNames = filePaths.Select(x => Path.GetFileNameWithoutExtension(x) ?? throw new UnreachableException()).ToArray();

		return (await Task.WhenAll(fileContents))
			.Pair(fileNames)
			.Select(x => new ScannedMatch { Name = x.second, Content = x.first })
			.OrderByDescending(x => x.Name)
			.ToArray();
	}

	private static async Task SaveMatchToFile(ScannedMatch scannedMatch) {

		Directory.CreateDirectory(MatchFileDirectoryPath);

		string fileName = $"{scannedMatch.Name}{FileExtension}";
		string filePath = Path.Combine(MatchFileDirectoryPath, fileName);

		await File.WriteAllTextAsync(filePath, scannedMatch.Content);
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public readonly struct ScannedMatch {

	public bool Equals(ScannedMatch other) {
		return Content == other.Content;
	}

	public override bool Equals(object? obj) {
		return obj is ScannedMatch other && Equals(other);
	}

	public override int GetHashCode() {
		return Content.GetHashCode();
	}

	public required string Name { get; init; }

	public required string Content { get; init; }

	public static bool operator ==(ScannedMatch left, ScannedMatch right) {
		return left.Equals(right);
	}

	public static bool operator !=(ScannedMatch left, ScannedMatch right) {
		return !left.Equals(right);
	}

}



// this is a fancy one for later
//public class MatchSummary {

//    public required DateTime ScanTime { get; init; }

//    public required uint MatchNumber { get; init; }

//    public required uint TeamNumber { get; init; }

//    public required string ScoutedBy { get; init; }

//    public MatchSummary(MatchData matchData) {

//    }

//}