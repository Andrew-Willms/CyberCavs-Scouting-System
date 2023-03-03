﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Storage;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Pages; 



public partial class QrCodePage : ContentPage, INotifyPropertyChanged {

	public static string Route => "QrCode";
	private static readonly string MatchFileDirectoryPath = Path.Combine(FileSystem.Current.CacheDirectory, "MatchData");
	private const string FileExtension = ".csvl";

	private bool IsActuallyRefreshing;
	private bool _IsRefreshing;
	public bool IsRefreshing {
		get => _IsRefreshing;
		set {
			_IsRefreshing = value;
			OnPropertyChanged(nameof(IsRefreshing)); }
	}

	public ObservableCollection<SerializedMatch> SavedMatches { get; } = new();



	public QrCodePage() {

		BindingContext = this;
		InitializeComponent();
	}



	private async Task Refresh() {

		await Dispatcher.DispatchAsync(async () => {

			if (IsActuallyRefreshing) {
				return;
			}

			IsRefreshing = false;
			IsActuallyRefreshing = true;

			SavedMatches.Clear();
			foreach (SerializedMatch match in await GetSavedGetScannedMatchesFromFiles()) {
				SavedMatches.Add(match);
			}

			IsActuallyRefreshing = false;
			IsRefreshing = false;
		});
	}

	private static async Task<SerializedMatch[]> GetSavedGetScannedMatchesFromFiles() {

		return new[] {
			new SerializedMatch { Name = "Name 1", Content = "some content here 1"},
			new SerializedMatch { Name = "Name 2", Content = "some content here 2"},
			new SerializedMatch { Name = "Name 3", Content = "some content here 3"},
			new SerializedMatch { Name = "Name 4", Content = "some content here 4"},
		};

		Directory.CreateDirectory(MatchFileDirectoryPath);

		string[] filePaths = Directory.GetFiles(MatchFileDirectoryPath, $"*{FileExtension}", SearchOption.TopDirectoryOnly);

		Task<string>[] fileContents = filePaths.Select(fileName => File.ReadAllTextAsync(fileName)).ToArray();
		string[] fileNames = filePaths.Select(x => Path.GetFileNameWithoutExtension(x) ?? throw new UnreachableException()).ToArray();

		return (await Task.WhenAll(fileContents))
			.Pair(fileNames)
			.Select(x => new SerializedMatch { Name = x.second, Content = x.first })
			.ToArray();
	}

	private async Task DeleteMatch(SerializedMatch match) {

		if (!SavedMatches.Contains(match)) {

			await Shell.Current.DisplayAlert(
				"Error",
				"The match you are trying to delete was not found in the list of matches." +
				"(Tell Andrew if this happens because it really shouldn't).",
				"Continue on with life I guess.");

			return;
		}

		SavedMatches.Remove(match);

		string[] fileNames = Directory.GetFiles(MatchFileDirectoryPath, $"*{FileExtension}", SearchOption.TopDirectoryOnly);

		foreach (string filePath in fileNames) {

			if (match.Content != await File.ReadAllTextAsync(filePath)) {
				continue;
			}

			File.Delete(filePath);
			return;
		}

		//TODO maybe throw an error if code execution gets here.
	}



	private async void QrCodePage_OnLoaded(object? sender, EventArgs e) {
		await Refresh();
	}

	private async void SavedMatchesView_OnRefreshing(object? sender, EventArgs e) {
		await Refresh();
	}

	private async void ViewMatch_OnClicked(object? sender, EventArgs e) {

		Button button = sender as Button ?? throw new ArgumentException();
		SerializedMatch scannedMatch = button.BindingContext is SerializedMatch match ? match : throw new ArgumentException();

		Dictionary<string, object> parameters = new() {
			{ MatchQrCodePage.SerializedMatchNavigationParameterName, scannedMatch },
			{ MatchQrCodePage.MatchDeleterNavigationParameterName, DeleteMatch }
		};

		await Shell.Current.GoToAsync(MatchQrCodePage.RouteFromQrCodePage, parameters);
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public readonly struct SerializedMatch {

	public bool Equals(SerializedMatch other) {
		return Content == other.Content;
	}

	public override bool Equals(object? obj) {
		return obj is SerializedMatch other && Equals(other);
	}

	public override int GetHashCode() {
		return Content.GetHashCode();
	}

	public required string Name { get; init; }

	public required string Content { get; init; }

	public static bool operator ==(SerializedMatch left, SerializedMatch right) {
		return left.Equals(right);
	}

	public static bool operator !=(SerializedMatch left, SerializedMatch right) {
		return !left.Equals(right);
	}

}