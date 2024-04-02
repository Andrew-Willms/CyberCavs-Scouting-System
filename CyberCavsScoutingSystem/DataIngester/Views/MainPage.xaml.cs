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

	private File? _SelectedDirectory;
	public File? SelectedDirectory {
		get => _SelectedDirectory;
		set {
			_SelectedDirectory = value;
			OnPropertyChanged(nameof(SelectedDirectory));
			OnPropertyChanged(nameof(RemoveButtonEnabled));
		}
	}

	public bool RemoveButtonEnabled => SelectedDirectory is not null;

	public ObservableCollection<File> SourceDirectories { get; } = new() {
		new() { Path = @"\Internal shared storage\Android\data\CCSS.QrCodeScanner\files\Documents\Data.csv" },
		new() { Path = @"\Internal storage\Android\data\CCSS.QrCodeScanner\files\Documents\Data.csv" },
		new() { Path = @"\Phone\Android\data\CCSS.QrCodeScanner\files\Documents\Data.csv" }
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

		while (LogMessages.Count > 100) {
			LogMessages.RemoveAt(0);
		}
	}

	private async void CopyData(object? sender, ElapsedEventArgs eventArgs) {

		if (!await Mutex.WaitAsync(TimeSpan.FromMilliseconds(400))) {
			return;
		}

		UpdateSourceDirectoryAccessibilities();

		List<string>? targetFileContents = await GetExistingMatchDataFromTargetFile();

		if (targetFileContents is null) {
			// this is an error state, if there is no match data the function will return an empty list
			return;
		}

		List<string> matchDataFromDevices = await GetMatchDataFromSourceDirectories();

		matchDataFromDevices.PruneEntriesFrom(targetFileContents);

		await WriteMatchDataToTargetFile(matchDataFromDevices.ToReadOnly());

		Mutex.Release();
	}

	private void UpdateSourceDirectoryAccessibilities() {

		foreach (File sourceDirectory in SourceDirectories) {

			sourceDirectory.IsAccessible = MtpService.GetDevices().Any(device => device.FileExistsSafe(sourceDirectory.Path));
		}
	}

	private async Task<List<string>?> GetExistingMatchDataFromTargetFile() {

		if (!System.IO.File.Exists(TargetFile)) {

			string? targetFileDirectory;
			try {
				targetFileDirectory = Path.GetDirectoryName(TargetFile);
			} catch {
				Logger($"The target file path \"{TargetFile}\" is invalid.");
				return null;
			}

			try {
				if (targetFileDirectory is not null) {
					Directory.CreateDirectory(targetFileDirectory);
				}

			} catch {
				Logger($"The directory of the target file \"{targetFileDirectory}\" could not be created.");
				return null;
			}

			try {
				await System.IO.File.WriteAllTextAsync(
					TargetFile,
					"ScouterInitials    Event    Match    Team    LeaveStartingZone    AutoNotes    Note1    Note2    Note3    Note4    Note5    Note6    Note7    Note8    FieldIgnored    AmpScores    SpeakerScores    NotesFed    EndGameStatus    TrapNote    Died    Comments\r\n");

			} catch {
				Logger($"Target File \"{TargetFile}\" does not exist and could not be created.");
				return null;
			}
		}

		string[]? fileContents = null;
		try {
			fileContents = await System.IO.File.ReadAllLinesAsync(TargetFile);
		} catch {
			Logger($"Target File \"{TargetFile}\" exists but could not be read.");
		}

		return fileContents?.Where(x => !string.IsNullOrEmpty(x)).ToList();
	}

	private async Task<List<string>> GetMatchDataFromSourceDirectories() {

		List<string> matchData = new();

		foreach (File file in SourceDirectories.Where(directory => directory.IsAccessible)) {

			foreach (MediaDevice device in MtpService.GetDevices()) {

				if (!device.FileExistsSafe(file.Path)) {
					continue;
				}

				IResult<string> result = await device.ReadFromMtpDevice(file.Path);

				if (result is IResult<string>.Error error) {
					Logger(error.Message);
					continue;
				}

				string fileContents = (result as IResult<string>.Success)?.Value ?? throw new UnreachableException();

				matchData.AddRange(fileContents.Split("\n").Where(x => !string.IsNullOrEmpty(x)));
			}
		}

		return matchData;
	}

	private async Task WriteMatchDataToTargetFile(ReadOnlyList<string> newMatchData) {

		if (newMatchData.IsEmpty()) {
			return;
		}

		newMatchData.Foreach(x => Logger($"Writing match \"{x}\"data to target file."));

		try {

			string fileContents = await System.IO.File.ReadAllTextAsync(TargetFile);

			if (!fileContents.IsEmpty() && !fileContents.EndsWith('\n') && !fileContents.EndsWith('\r')) {
				await System.IO.File.AppendAllTextAsync(TargetFile, "\n");
			}

			await System.IO.File.AppendAllLinesAsync(TargetFile, newMatchData);

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
		File sourceFile = sourceDirectoryView.BindingContext as File ?? throw new UnreachableException();
		SelectedDirectory = sourceFile;
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

	public static bool FileExistsSafe(this MediaDevice device, string filePath) {

		try {
			return device.FileExists(filePath);
		} catch {
			return false;
		}
	}

}