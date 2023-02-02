using Microsoft.Maui.Controls;

namespace QrCodeScanner;



public partial class App : Application {
	
	public App() {

		InitializeComponent();

		MainPage = new AppShell();
	}

}