using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using CCSSDomain;
using GameMaker.Views;

namespace GameMaker;

public static class ApplicationManager {

	public static GameProject GameProject { get; private set; } = new();

	public static MainWindow Window { get; private set; } = new();

	public static bool ApplicationStartup() {

		Window.Show();

		return true;
	}

	public static void SaveGameProject() {

	}

	public static void LoadGameProject(string path) {

	}

}