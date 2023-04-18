using Microsoft.Maui.Controls;

namespace BluetoothTest;



public partial class App : Application {

	public App() {
		InitializeComponent();

		MainPage = new AppShell();
	}

}