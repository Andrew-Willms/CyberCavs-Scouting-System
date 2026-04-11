using Microsoft.Maui.Controls;
using ScoutingApp.Views.Pages.Flyout;

namespace ScoutingApp;



public partial class AppShell : Shell {

	public static string MatchRoute => "Match";

	public AppShell() {

		Routing.RegisterRoute($"{MatchQrCodePage.Route}", typeof(MatchQrCodePage));

		InitializeComponent();

		GoToAsync(MatchRoute);
	}

}