using Microsoft.Maui.Controls;
using ScoutingApp.AppManagement;

namespace ScoutingApp;



public partial class AppShell : Shell, IMainView {

	public Page AsPage() => this;

	public AppShell() {
		InitializeComponent();
	}

}