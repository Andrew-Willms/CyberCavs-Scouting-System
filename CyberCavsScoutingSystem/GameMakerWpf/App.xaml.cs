using System.Windows;
using GameMakerWpf.AppManagement;

namespace GameMakerWpf;



public partial class App : Application {

	public static readonly AppManager Manager = new();

	private void ApplicationStartup(object sender, StartupEventArgs e) {

		Manager.ApplicationStartup();

	}

}