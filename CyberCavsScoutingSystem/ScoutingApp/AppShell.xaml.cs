using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;
using ScoutingApp.Views.Pages;

namespace ScoutingApp;



public partial class AppShell : Shell, IMainView {

	public Page AsPage() => this;

	public AppShell() {

		Routing.RegisterRoute($"{MatchQrCodePage.Route}", typeof(MatchQrCodePage));

		InitializeComponent();
	}

}