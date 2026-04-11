using Microsoft.Maui.Controls;
using ScoutingApp.Views.Pages.Flyout;

namespace ScoutingApp;



public partial class AppShell : Shell {

	public static string MatchRoute => "Match";

	public AppShell() {

		Routing.RegisterRoute($"{MatchDetailsPage.Route}", typeof(MatchDetailsPage));

		InitializeComponent();

		GoToAsync(MatchRoute);
	}

}