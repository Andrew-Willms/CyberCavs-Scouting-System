using System;
using System.Diagnostics;
using System.Linq;
using CCSSDomain.Serialization;
using Database;
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



	private async void CameraBarcodeReaderView_OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e) {

		try {

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

			switch (await DataStore.AddMatchDataFromOtherDevice(matchData)) {

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
		} catch (Exception exception) {
			// ignore
		}

	}

	private static bool IsValidQrCode(BarcodeResult qrCode) {

		return
			!qrCode.Value.All(character => character is '0') &&
			qrCode.Value.Length > 25;
	}

	private void ExportButton_OnClicked(object? sender, EventArgs e) {
		throw new NotImplementedException();
	}

}