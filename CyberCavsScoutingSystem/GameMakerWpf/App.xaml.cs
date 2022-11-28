using System.Windows;
using GameMakerWpf.ApplicationManagement;

namespace GameMakerWpf;



public partial class App : Application {

	private void ApplicationStartup(object sender, StartupEventArgs e) {

		ApplicationManager.ApplicationStartup();

	}

}