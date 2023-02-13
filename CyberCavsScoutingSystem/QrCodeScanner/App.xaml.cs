using Microsoft.Maui.Controls;
using QrCodeScanner.Views;

namespace QrCodeScanner;



public partial class App : Application {
	
	public App() {

		InitializeComponent();

		MainPage = new AppShell();
	}

}