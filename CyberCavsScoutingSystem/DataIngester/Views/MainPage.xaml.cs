using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataIngester.Services;
using MediaDevices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace DataIngester.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	private FileSystemItem _TargetFile = new() { Path = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\CCSS\ScoutingData\Data.csv") };
	public FileSystemItem TargetFile {
		get => _TargetFile;
		set {
			_TargetFile = value;
			OnPropertyChanged(nameof(TargetFile));
		}
	}

	private FileSystemItem? _SelectedDirectory;
	public FileSystemItem? SelectedDirectory {
		get => _SelectedDirectory;
		set {
			_SelectedDirectory = value;
			OnPropertyChanged(nameof(SelectedDirectory));
		}
	}

	public ObservableCollection<FileSystemItem> SourceDirectories { get; } = new();

	public ObservableCollection<string> LogMessages { get; } = new();
	public Action<string> Logger => LogMessages.Add;



	public MainPage() {

		InitializeComponent();
		BindingContext = this;

		SourceDirectories.Add(new() { Path = @"\Internal shared storage\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" });

		Dispatcher.StartTimer(new(0, 0, 0, 1), () => {
			Dispatcher.DispatchAsync(async () => await RunBackgroundTask(SourceDirectories.ToReadOnly(), TargetFile.Path));
			return true;
		});
	}



	private static async Task RunBackgroundTask(ReadOnlyList<FileSystemItem> sourceDirectories, string targetFilePath) {

		UpdateSourceDirectoryAccessibilities(sourceDirectories);

		IResult<List<string>> result = await GetExistingMatchDataFromTargetFile(targetFilePath);

		List<string>? matchDataFileLines = (result as IResult<List<string>>.Success)?.Value;
		if (matchDataFileLines is null) {
			return;
		}

		ReadOnlyList<string> matchDataFromDevices = await GetMatchDataFromSourceDirectories(sourceDirectories);

		matchDataFileLines.AddUniqueItems(matchDataFromDevices);

		await WriteMatchDataToTargetFile(matchDataFileLines);
	}

	private static void UpdateSourceDirectoryAccessibilities(IEnumerable<FileSystemItem> sourceDirectories) {

		foreach (FileSystemItem sourceDirectory in sourceDirectories) {

			sourceDirectory.IsAccessible = StaticServiceResolver.Resolve<IMtpDeviceService>()
				.GetDevices().Any(device => device.DirectoryExists(sourceDirectory.Path));
		}
	}

	private static async Task<ReadOnlyList<string>> GetMatchDataFromSourceDirectories(IEnumerable<FileSystemItem> sourceDirectories) {

		List<string> matchData = new();

		foreach (FileSystemItem directory in sourceDirectories.Where(directory => directory.IsAccessible)) {

			foreach (MediaDevice device in StaticServiceResolver.Resolve<IMtpDeviceService>().GetDevices()) {

				IResult<string> test = await device.ReadFromMtpDevice(directory.Path);

				switch (test) {

					case IResult<string>.Error:
						//TODO log
						continue;

					case IResult<string>.Success success:
						//TODO log
						matchData.Add(success.Value);
						continue;

					default:
						throw new UnreachableException();
				}

			}
		}

		return matchData.ToReadOnly();
	}

	private static async Task<IResult<List<string>>> GetExistingMatchDataFromTargetFile(string targetFilePath) {

		if (!File.Exists(targetFilePath)) {

			try {
				File.Create(targetFilePath, App.GameSpecification.GetHashCode());
			} catch {
				return new IResult<List<string>>.Error($"Target File \"{targetFilePath}\" does not exist and could not be created.");
			}
		}

		string[] fileContents;
		try {
			fileContents = await File.ReadAllLinesAsync(targetFilePath);
		} catch {
			return new IResult<List<string>>.Error($"Target File \"{targetFilePath}\" exists but could not be read.");
		}

		return new IResult<List<string>>.Success { Value = fileContents.ToList() };
	}

	private static async Task WriteMatchDataToTargetFile(IEnumerable<string> matchData) {
		throw new NotImplementedException();
	}



	private void DeleteSourceFile_OnClick(object? sender, EventArgs e) {

		if (SelectedDirectory is null) {
			throw new InvalidOperationException();
		}

		SourceDirectories.Remove(SelectedDirectory);
	}

	private void AddSourceFileButton_OnClick(object? sender, EventArgs e) {
		
		SourceDirectories.Add(new());
	}

	private void SourceFileCollectionViewItem_OnFocus(object? sender, FocusEventArgs e) {

		FileSystemItemView sourceDirectoryView = sender as FileSystemItemView ?? throw new ArgumentException();
		FileSystemItem sourceDirectory = sourceDirectoryView.BindingContext as FileSystemItem ?? throw new ArgumentException();
		SelectedDirectory = sourceDirectory;
	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}



public static class MtpDeviceExtensions {

	public static async Task<IResult<string>> ReadFromMtpDevice(this MediaDevice device, string filePath) {

		MemoryStream memoryStream = new();

		try {
			device.DownloadFile(filePath, memoryStream);
		} catch {
			return new IResult<string>.Error("The specified file could not be downloaded from the device.");
		}

		memoryStream.Position = 0;
		StreamReader streamReader = new(memoryStream);
		string matchData;

		try {
			matchData = await streamReader.ReadToEndAsync();
		} catch {
			return new IResult<string>.Error("The specified file was copied from the device but could not be read.");
		}

		return new IResult<string>.Success(matchData);
	}

	public static bool DirectoryExistsSafe(this MediaDevice device, string directoryPath) {

		try {
			return device.DirectoryExists(directoryPath);
		} catch {
			return false;
		}
	}

}