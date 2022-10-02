using System;
using GameMakerWpf.Domain;
using GameMakerWpf.DomainData;
using GameMakerWpf.Views;

namespace GameMakerWpf;



public static class ApplicationManager {

	public static GameEditingData Game { get; private set; } = DefaultEditingDataValues.GetDefaultEditingData();

	private static MainWindow Window { get; } = new();

	public static bool ApplicationStartup() {

		Window.Show();

		return true;
	}




	public static void SaveGameProject() {
		throw new NotImplementedException();
	}

	public static void OpenGameProject(string path) {
		throw new NotImplementedException();
	}

	public static void NewGameProject() {
		throw new NotImplementedException();
	}



	public static void AddAlliance() {
		Game.Alliances.Add(DefaultEditingDataValues.GetNewAlliance(Game));
	}

	public static void RemoveAlliance(AllianceEditingData alliance) {
		Game.Alliances.Remove(alliance);
	}

}