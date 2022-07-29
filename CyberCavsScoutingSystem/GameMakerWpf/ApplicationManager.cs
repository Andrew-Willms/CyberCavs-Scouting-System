using System;
using CCSSDomain;
using GameMakerWpf.Views;

namespace GameMakerWpf;



public static class ApplicationManager {

	public static GameProject GameProject { get; private set; } = new();

	public static MainWindow Window { get; private set; } = new();

	public static bool ApplicationStartup() {

		Window.Show();

		return true;
	}

	public static void SaveGameProject() {
		throw new NotImplementedException();
	}

	public static void LoadGameProject(string path) {
		throw new NotImplementedException();
	}

}