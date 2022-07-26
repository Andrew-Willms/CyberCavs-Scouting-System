using System.Windows;

namespace GameMakerWpf;



/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

	private void ApplicationStartup(object sender, StartupEventArgs e) {

		ApplicationManager.ApplicationStartup();

	}

}