using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ZXing.Net.Maui;

namespace QrCodeScanner.Views;



[QueryProperty(nameof(ScanAdder), AddMatchNavigationParameterName)]
public partial class MatchScannerPage : ContentPage, INotifyPropertyChanged {

	public static string Route => nameof(MatchScannerPage);
	public const string AddMatchNavigationParameterName = "AddMatch";



	public Func<string, Task<bool>> ScanAdder { get; init; } = null!;

	private int _QrCodeCount;
	public int QrCodeCount {
		get => _QrCodeCount;
		set {
			_QrCodeCount = value;
			OnPropertyChanged(nameof(QrCodeCount));
		}
	}

	private string LastLastQrCodeScanned = "";
	public string LastQrCodeScanned {
		get => LastLastQrCodeScanned;
		set {
			LastLastQrCodeScanned = value;
			OnPropertyChanged(nameof(LastQrCodeScanned));
		}
	}



	public MatchScannerPage() {

		BindingContext = this;
		InitializeComponent();

		QrCodeReader.Options = new() {
			Formats = BarcodeFormats.TwoDimensional,
			TryHarder = true,
			AutoRotate = true,
			Multiple = true
		};
	}



	private async void CameraBarcodeReaderView_OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e) {

		string? data = e.Results.FirstOrDefault(IsValidQrCode)?.Value;

		if (data is null) {
			return;
		}

		if (!await ScanAdder(data)) {
			return;
		}

		QrCodeCount++;
		LastQrCodeScanned = data;
	}

	private static bool IsValidQrCode(BarcodeResult qrCode) {

		return
			!qrCode.Value.All(character => character is '0') &&
			qrCode.Value.Length > 25;

	}



	public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}