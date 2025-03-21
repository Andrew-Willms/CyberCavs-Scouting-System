using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CCSSDomain.Serialization;
using Database;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using ZXing.Net.Maui;

namespace ScoutingApp.Views.Pages.Flyout;



public partial class QrCodeScanner : ContentPage {

	public static string Route => "QrCodeScanner";

	private IAppManager AppManager { get; }
	private IDataStore DataStore { get; }
	private IErrorPresenter ErrorPresenter { get; }

	public int QrCodeCount {
		get;
		set {
			field = value;
			OnPropertyChanged();
		}
	}

	public string LastQrCodeScanned {
		get;
		set {
			field = value;
			OnPropertyChanged();
		}
	} = "";



	public QrCodeScanner(IAppManager appManager, IDataStore dataStore, IErrorPresenter errorPresenter) {

		AppManager = appManager;
		DataStore = dataStore;
		ErrorPresenter = errorPresenter;

		BindingContext = this;
		InitializeComponent();

		QrCodeReader.Options = new() {
			Formats = BarcodeFormats.TwoDimensional,
			TryHarder = true,
			AutoRotate = true,
			Multiple = false
		};
	}



	private void CameraBarcodeReaderView_OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e) {

		string? qrCodeString = e.Results.FirstOrDefault(IsValidQrCode)?.Value;

		if (qrCodeString is null) {
			return;
		}

		if (AppManager.GameSpecification is null) {
			ErrorPresenter.DisplayError("Game Specification Null",
				"The GameSpecification is null, this shouldn't be the case.");
			return;
		}

		MatchDataDto? matchData = MatchDataDtoToCsv.Deserialize(qrCodeString, AppManager.GameSpecification);

		if (matchData is null) {
			ErrorPresenter.DisplayError("Invalid QR Code", "The QR code data could not be converted into a match.");
			return;
		}

		IDataStore.AddMatchDataResult safeResult = DataStore.AddMatchDataFromOtherDevice(matchData).Result;

		MainThread.BeginInvokeOnMainThread(() => {

			switch (safeResult) {

				case IDataStore.AddMatchDataResult.Success:
					QrCodeCount++;
					LastQrCodeScanned = qrCodeString;
					break;

				case IDataStore.AddMatchDataResult.Duplicate:
					LastQrCodeScanned = "Duplicate";
					break;

				case IDataStore.AddMatchDataResult.Other:
					ErrorPresenter.DisplayError("Unknown Error", "Uh-oh.");
					break;

				default:
					throw new UnreachableException();
			}
		});
	}

	private static bool IsValidQrCode(BarcodeResult qrCode) {

		return
			!qrCode.Value.All(character => character is '0') &&
			qrCode.Value.Length > 25;
	}

	private async void ExportButton_OnClicked(object? sender, EventArgs e) {

		if (AppManager.GameSpecification is null) {
			ErrorPresenter.DisplayError("Game Specification Null",
				"The GameSpecification is null, this shouldn't be the case.");
			return;
		}

		List<MatchDataDto>? matchData = await DataStore.GetMatchData();

		if (matchData is null) {
			ErrorPresenter.DisplayError("Error Loading Match Data",
				"Could not load the match data from the database while trying to export to CSV.");
			return;
		}

		StringBuilder stringBuilder = new(MatchDataToCsv.GetCsvHeaders(AppManager.GameSpecification));
		foreach (MatchDataDto matchDataDto in matchData) {
			stringBuilder.Append('\n');
			stringBuilder.Append(MatchDataToCsv.Serialize(matchDataDto.MatchData));
		}

		string saveDirectory = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments)!.AbsolutePath;
		string path = Path.Combine(saveDirectory, $"Match Data {DateTime.Now:yyyy-MM-dd HH_mm_ss}.csv");
		await File.WriteAllTextAsync(path, stringBuilder.ToString());
	}

}