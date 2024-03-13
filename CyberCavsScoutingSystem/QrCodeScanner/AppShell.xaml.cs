using Microsoft.Maui.Controls;
using QrCodeScanner.Views;

namespace QrCodeScanner;



public partial class AppShell : Shell {

	public AppShell() {

		InitializeComponent();

		Routing.RegisterRoute($"{MatchDetailsPage.Route}", typeof(MatchDetailsPage));
		Routing.RegisterRoute($"{MatchScannerPage.Route}", typeof(MatchScannerPage));
	}

}