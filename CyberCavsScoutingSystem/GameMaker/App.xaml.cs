using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace GameMaker {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private void ApplicationStartup(object sender, StartupEventArgs e) {

			ApplicationManager.ApplicationStartup();

			MainWindow = new MainWindow(); // If I use the condensed new() syntax it creates a blank window.
			MainWindow.Show();

		}

	}

}
