using Microsoft.Maui.Controls;

namespace QrCodeScanner.Views;



public partial class AppShell : Shell {

	public AppShell() {

		InitializeComponent();

		Routing.RegisterRoute($"{MatchDetailsPage.Route}", typeof(MatchDetailsPage));
		Routing.RegisterRoute($"{MatchScannerPage.Route}", typeof(MatchScannerPage));
	}

}