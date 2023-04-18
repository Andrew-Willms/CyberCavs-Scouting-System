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



	public Func<ScannedMatch, Task> ScanAdder { get; init; } = null!;

	public bool CanSave => QrCodeData != "" && !QrCodeData.All(character => character is '0');

	private string _QrCodeData = "";
	public string QrCodeData {
		get => _QrCodeData;
        set {
            _QrCodeData = value;
			OnPropertyChanged(nameof(QrCodeData));
			OnPropertyChanged(nameof(CanSave));
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



	private void CameraBarcodeReaderView_OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e) {

        QrCodeData = e.Results.First().Value;
    }

    private async void Button_OnClicked(object? sender, EventArgs e) {

        ScannedMatch match = new() {
	        Name = $"{DateTime.Now:yyyy-MM-dd HH.mm.ss}",
	        Content = QrCodeData
        };

        QrCodeData = "";
		await ScanAdder(match);
    }



    public new event PropertyChangedEventHandler? PropertyChanged;

	private new void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}