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

		DateTime previousTime = DateTime.Now;
		Trace.WriteLine($"{previousTime:HH:mm:ss.fffffff} entering function");

		if (!await Mutex.WaitAsync(TimeSpan.FromMilliseconds(400))) {
			Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} early exit");
			return;
		}

		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} mutex acquired");
		previousTime = DateTime.Now;

		UpdateSourceDirectoryAccessibilities();
		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} source directories updated");
		previousTime = DateTime.Now;

		List<string>? targetFileContents = await GetExistingMatchDataFromTargetFile();
		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} existing file contents read");
		previousTime = DateTime.Now;

		if (targetFileContents is null) {
			return;
		}

		List<(Directory sourceDirectory, string fileContents)> matchDataFromDevices = await GetMatchDataFromSourceDirectories();
		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} devices read");
		previousTime = DateTime.Now;

		matchDataFromDevices.PruneEntriesFrom(targetFileContents, (tuple, matchDataFileLine) => tuple.fileContents == matchDataFileLine);
		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} duplicate entries removed");
		previousTime = DateTime.Now;

		await WriteMatchDataToTargetFile(matchDataFromDevices.ToReadOnly());
		Trace.WriteLine($"{DateTime.Now - previousTime:s\\.fffffff} file written");

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

		//IEnumerable<Task<(Directory, string)?>> taskList = SourceDirectories
		//	.Where(directory => directory.IsAccessible)
		//	.SelectMany(directory => MtpService.GetDevices(), (directory, device) => (directory, device))
		//	.SelectMany(x => x.device.EnumerateFiles(x.directory.Path), (x, filePath) => (x.directory, x.device, filePath))
		//	.Select(x => (x.directory, x.device.ReadFromMtpDevice(x.filePath)))
		//	.Select(x => x.Item2.ContinueWith<(Directory, string)?>(task => task.Result switch {
		//		IResult<string>.Error => null,
		//		IResult<string>.Success success => (x.directory, success.Value),
		//		_ => throw new UnreachableException()
		//	}));

		//return (await Task.WhenAll(taskList))
		//	.Where(x => x is not null)
		//	.Select(x => (x!.Value.Item1, x.Value.Item2))
		//	.ToList();

		List<Task<(Directory, string)?>> taskList = new();

		foreach (Directory directory in SourceDirectories.Where(directory => directory.IsAccessible)) {

			foreach (MediaDevice device in MtpService.GetDevices()) {

				foreach (string filePath in device.EnumerateFiles(directory.Path)) {

					Task<IResult<string>> getFileContentsTask = device.ReadFromMtpDevice(filePath);

					Trace.WriteLine($"{filePath} reading started");

					taskList.Add(getFileContentsTask.ContinueWith<(Directory, string)?>(x => {

						Trace.WriteLine($"{filePath} reading finished");

						switch (getFileContentsTask.Result) {

							case IResult<string>.Error error:
								Logger(error.Message);
								return null;

							case IResult<string>.Success success:
								return (directory, success.Value);

							default:
								throw new UnreachableException();
						}
					}));
				}
			}
		}

		await Task.WhenAll(taskList);

		return taskList
			.Select(x => x.Result)
			.Where(x => x is not null)
			.Select(x => (x!.Value.Item1, x.Value.Item2))
			.ToList();
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