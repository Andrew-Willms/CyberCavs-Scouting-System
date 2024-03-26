using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DataIngester.Services;
using MediaDevices;
using Microsoft.Maui.Controls;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;
using Exception = System.Exception;
using Timer = System.Timers.Timer;

namespace DataIngester.Views;



public partial class MainPage : ContentPage, INotifyPropertyChanged {

	private readonly IMtpDeviceService MtpService;

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

	public ObservableCollection<Directory> SourceDirectories { get; } = new() {
		new() { Path = @"\Internal shared storage\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" },
		new() { Path = @"\Internal storage\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" },
		new() { Path = @"\Phone\Android\data\CCSS.QrCodeScanner\files\Documents\CCSS.QrCodeScanner" }
	};

	public ObservableCollection<string> LogMessages { get; } = new();
	private Action<string> Logger => text => LogMessages.Add(DateTime.Now + ": " + text);

	private static readonly SemaphoreSlim Mutex = new(1);

	private readonly Timer Timer = new(TimeSpan.FromSeconds(1));



	public MainPage(IMtpDeviceService mtpService) {

		MtpService = mtpService;

		InitializeComponent();
		BindingContext = this;

		Timer.Elapsed += TrimLog;
		Timer.Elapsed += CopyData;
		Timer.Start();
	}



	private void TrimLog(object? sender, ElapsedEventArgs eventArgs) {

		if (LogMessages.Count > 100) {
			LogMessages.RemoveAt(0);
		}
	}

	private async void CopyData(object? sender, ElapsedEventArgs eventArgs) {

		if (Mutex.CurrentCount == 0) {
			return;
		}

		await Mutex.WaitAsync();
		
		try {

			UpdateSourceDirectoryAccessibilities();

			List<string>? targetFileContents = await GetExistingMatchDataFromTargetFile();
			if (targetFileContents is null) {
				return;
			}

			List<(Directory sourceDirectory, string fileContents)> matchDataFromDevices = await GetMatchDataFromSourceDirectories();

			matchDataFromDevices.PruneEntriesFrom(targetFileContents, (tuple, matchDataFileLine) => tuple.fileContents == matchDataFileLine);

			await WriteMatchDataToTargetFile(matchDataFromDevices.ToReadOnly());

		} catch (Exception exception) {

			Trace.WriteLine(exception);
		}

		Mutex.Release();
	}

	private void UpdateSourceDirectoryAccessibilities() {

		foreach (Directory sourceDirectory in SourceDirectories) {

			sourceDirectory.IsAccessible = MtpService.GetDevices().Any(device => device.DirectoryExistsSafe(sourceDirectory.Path));
		}
	}

	private async Task<List<string>?> GetExistingMatchDataFromTargetFile() {

		if (!File.Exists(TargetFile)) {

			string? targetFileDirectory;
			try {
				targetFileDirectory = Path.GetDirectoryName(TargetFile);
			} catch {
				Logger($"The target file path \"{TargetFile}\" is invalid.");
				return null;
			}

			try {
				if (targetFileDirectory is not null) {
					System.IO.Directory.CreateDirectory(targetFileDirectory);
				}

			} catch {
				Logger($"The directory of the target file \"{targetFileDirectory}\" could not be created.");
				return null;
			}

			try {
				File.Create(TargetFile);
			} catch {
				Logger($"Target File \"{TargetFile}\" does not exist and could not be created.");
				return null;
			}
		}

		string[]? fileContents = null;
		try {
			fileContents = await File.ReadAllLinesAsync(TargetFile);
		} catch {
			Logger($"Target File \"{TargetFile}\" exists but could not be read.");
		}

		return fileContents?.ToList();
	}

	private async Task<List<(Directory, string)>> GetMatchDataFromSourceDirectories() {

		List<(Directory, string)> matchData = new();

		foreach (Directory directory in SourceDirectories.Where(directory => directory.IsAccessible)) {

			foreach (MediaDevice device in MtpService.GetDevices()) {

				foreach (string filePath in device.EnumerateFiles(directory.Path)) {

					IResult<string> fileContents = await device.ReadFromMtpDevice(filePath);

					switch (fileContents) {

						case IResult<string>.Error error:
							Logger(error.Message);
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

	private async Task WriteMatchDataToTargetFile(ReadOnlyList<(Directory sourceDirectory, string fileContents)> newMatchData) {

		if (newMatchData.IsEmpty()) {
			return;
		}

		newMatchData.Foreach(x => Logger($"Writing match data from \"{x.sourceDirectory.Path}\" to \"{TargetFile}\"."));

		try {

			string fileContents = await File.ReadAllTextAsync(TargetFile);

			if (!fileContents.IsEmpty() && !fileContents.EndsWith('\n') && !fileContents.EndsWith('\r')) {
				await File.AppendAllTextAsync(TargetFile, "\n");
			}

			await File.AppendAllLinesAsync(TargetFile, newMatchData.Select(x => x.fileContents));

		} catch {
			Logger($"Could not write match data to the file \"{TargetFile}\".");
		}
	}



	private void DeleteSourceFile_OnClick(object? sender, EventArgs e) {

		if (SelectedDirectory is null) {
			throw new UnreachableException();
		}

		SourceDirectories.Remove(SelectedDirectory);
	}

	private void AddSourceFileButton_OnClick(object? sender, EventArgs e) {
		
		SourceDirectories.Add(new());
	}

	private void SourceFileCollectionViewItem_OnFocus(object? sender, FocusEventArgs e) {

		FileSystemItemView sourceDirectoryView = sender as FileSystemItemView ?? throw new UnreachableException();
		Directory sourceDirectory = sourceDirectoryView.BindingContext as Directory ?? throw new UnreachableException();
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
		} catch (Exception exception) {

			Trace.WriteLine(exception);
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