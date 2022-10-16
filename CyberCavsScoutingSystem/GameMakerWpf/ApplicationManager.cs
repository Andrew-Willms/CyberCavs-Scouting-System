using System;
using GameMakerWpf.Domain;
using GameMakerWpf.DomainData;
using GameMakerWpf.Views;

namespace GameMakerWpf;



public static class ApplicationManager {

	public static GameEditingData GameEditingData { get; private set; } = DefaultEditingDataValues.DefaultEditingData;

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
		GameEditingData.AddAlliance(DefaultEditingDataValues.GetNewAlliance(GameEditingData));
	}

	public static void RemoveAlliance(AllianceEditingData alliance) {
		GameEditingData.RemoveAlliance(alliance);
	}



	public static void AddDataField() {
		GameEditingData.AddDataField(DefaultEditingDataValues.GetNewDataField(GameEditingData));
	}

	public static void RemoveDataField(GeneralDataFieldEditingData generalDataField) {
		GameEditingData.RemoveDataField(generalDataField);
	}

}