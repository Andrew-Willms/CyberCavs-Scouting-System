using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CCSSDomain.GameSpecification;
using DataIngester.Services;
using MediaDevices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace DataIngester.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	private string _TargetFile = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\CCSS\ScoutingData\Data.csv");
	public string TargetFile {
		get => _TargetFile;
		set {
			_TargetFile = value;
			OnPropertyChanged(nameof(TargetFile));
		}
	}

	private Directory? _SelectedDirectory;
	public Directory? SelectedDirectory {
		get => _SelectedDirectory;
		set {
			_SelectedDirectory = value;
			OnPropertyChanged(nameof(SelectedDirectory));
			OnPropertyChanged(nameof(RemoveButtonEnabled));
		}
	}

	public bool RemoveButtonEnabled => SelectedDirectory is not null;

	public ObservableCollection<Directory> SourceDirectories { get; } = new();

	public ObservableCollection<string> LogMessages { get; } = new();
	private Action<string> Logger => text => LogMessages.Add(DateTime.Now + ": " + text);



	public MainPage() {

		InitializeComponent();
		BindingContext = this;

		SourceDirectories.Add(new() { Path = @"\Internal shared storage\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" });
		SourceDirectories.Add(new() { Path = @"\Internal storage\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" });
		SourceDirectories.Add(new() { Path = @"\Phone\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" });

		Dispatcher.StartTimer(new(0, 0, 0, 1), () => {
			Dispatcher.DispatchAsync(async () => await RunBackgroundTask(SourceDirectories.ToReadOnly(), TargetFile, Logger));
			return true;
		});

		Dispatcher.StartTimer(new(0, 0, 0, 1), () => {

			while (LogMessages.Count > 100) {
				LogMessages.RemoveAt(0);
			}

			return true;
		});
	}



	private static async Task RunBackgroundTask(ReadOnlyList<Directory> sourceDirectories, string targetFilePath, Action<string> log) {

		UpdateSourceDirectoryAccessibilities(sourceDirectories);

		List<string>? targetFileContents = await GetExistingMatchDataFromTargetFile(targetFilePath, log);
		if (targetFileContents is null) {
			return;
		}

		List<(Directory sourceDirectory, string fileContents)> matchDataFromDevices = await GetMatchDataFromSourceDirectories(sourceDirectories, log);

		matchDataFromDevices.PruneEntriesFrom(targetFileContents, (tuple, matchDataFileLine) => tuple.fileContents == matchDataFileLine);

		await WriteMatchDataToTargetFile(targetFilePath, matchDataFromDevices.ToReadOnly(), log);
	}

	private static void UpdateSourceDirectoryAccessibilities(IEnumerable<Directory> sourceDirectories) {

		foreach (Directory sourceDirectory in sourceDirectories) {

			sourceDirectory.IsAccessible = StaticServiceResolver.Resolve<IMtpDeviceService>()
				.GetDevices().Any(device => device.DirectoryExistsSafe(sourceDirectory.Path));
		}
	}

	private static async Task<List<string>?> GetExistingMatchDataFromTargetFile(string targetFilePath, Action<string> log) {

		if (!File.Exists(targetFilePath)) {

			IResult<GameSpec> result = await App.GetGameSpec();

			if (result is IResult<GameSpec>.Error error) {
				log($"Error while parsing game specification file: \"{error.Message}\"");
				return null;
			}

			GameSpec gameSpec = (result as IResult<GameSpec>.Success)?.Value ?? throw new UnreachableException();

			string? targetFileDirectory;
			try {
				targetFileDirectory = Path.GetDirectoryName(targetFilePath);
			} catch {
				log($"The target file path \"{targetFilePath}\" is invalid.");
				return null;
			}

			try {
				if (targetFileDirectory is not null) {
					System.IO.Directory.CreateDirectory(targetFileDirectory);
				}

			} catch {
				log($"The directory of the target file \"{targetFileDirectory}\" could not be created.");
				return null;
			}

			try {
				await File.WriteAllTextAsync(targetFilePath, gameSpec.GetCsvHeaders());

			} catch {
				log($"Target File \"{targetFilePath}\" does not exist and could not be created.");
				return null;
			}
		}

		string[]? fileContents = null;
		try {
			fileContents = await File.ReadAllLinesAsync(targetFilePath);
		} catch {
			log($"Target File \"{targetFilePath}\" exists but could not be read.");
		}

		return fileContents?.ToList();
	}

	private static async Task<List<(Directory, string)>> GetMatchDataFromSourceDirectories(
		IEnumerable<Directory> sourceDirectories, Action<string> log) {

		List<(Directory, string)> matchData = new();

		foreach (Directory directory in sourceDirectories.Where(directory => directory.IsAccessible)) {

			foreach (MediaDevice device in StaticServiceResolver.Resolve<IMtpDeviceService>().GetDevices()) {

				foreach (string filePath in device.EnumerateFiles(directory.Path)) {

					IResult<string> fileContents = await device.ReadFromMtpDevice(filePath);

					switch (fileContents) {

						case IResult<string>.Error error:
							log(error.Message);
							continue;

						case IResult<string>.Success success:
							matchData.Add((directory, success.Value));
							continue;

						default:
							throw new UnreachableException();
					}
				}
			}
		}

		return matchData;
	}

	private static async Task WriteMatchDataToTargetFile(string targetFilePath, 
		ReadOnlyList<(Directory sourceDirectory, string fileContents)> newMatchData, Action<string> log) {

		newMatchData.Foreach(x => log($"Writing match data from \"{x.sourceDirectory.Path}\" to \"{targetFilePath}\"."));

		try {

			string fileContents = await File.ReadAllTextAsync(targetFilePath);

			if (!fileContents.EndsWith("\n")) {
				await File.AppendAllTextAsync(targetFilePath, "\n");
			}

			await File.AppendAllLinesAsync(targetFilePath, newMatchData.Select(x => x.fileContents));

		} catch {
			log($"Could not write match data to the file \"{targetFilePath}\".");
		}
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
		Directory sourceDirectory = sourceDirectoryView.BindingContext as Directory ?? throw new ArgumentException();
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