using Microsoft.Maui.Controls;

namespace ScheduleInputer;



public partial class App : Application {

	public App() {

		InitializeComponent();
		MainPage = new AppShell();
	}

}