using System;
using System.Collections.Generic;
using System.ComponentModel;
using GameMakerWpf.Domain;
using GameMakerWpf.DomainData;
using GameMakerWpf.Views;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using static GameMakerWpf.ISavePrompter;

namespace GameMakerWpf;



public static class ApplicationManager {

	private static GameEditingData _GameEditingData = DefaultEditingDataValues.DefaultEditingData;
	public static GameEditingData GameEditingData {

		get => _GameEditingData;

		private set {
			value.AnythingChanged.Subscribe(MarkProjectUnsaved);
			_GameEditingData = value;
			ProjectIsSaved = false;
			InvokeGameProjectChangeActions();
		}
	}

	private static readonly List<Action> GameProjectChangeActions = new();

	private static IGameMakerMainView MainView { get; } = new MainWindow();

	private static IGameSaver Saver { get; } = new GameProjectSaver();

	private static ISavePrompter SavePrompter { get; } = new GameSavePrompter();

	private static bool ProjectIsSaved { get; set; }



	public static void ApplicationStartup() {
		MainView.Show();
	}



	public static void RegisterGameProjectChangeAction(Action action) {
		GameProjectChangeActions.Add(action);
	}

	private static void InvokeGameProjectChangeActions() {

		foreach (Action action in GameProjectChangeActions) {
			action.Invoke();
		}
	}

	private static void MarkProjectUnsaved() {
		ProjectIsSaved = false;
	}



	public static void SaveGameProject() {

		if (!Saver.ProjectHasSaveLocation) {
			SaveGameProjectAs();
			return;
		}

		Saver.Save(GameEditingData);
		ProjectIsSaved = true;
	}

	public static void SaveGameProjectAs() {
		Saver.SaveAs(GameEditingData);
	}

	public static void OpenGameProject() {

		if (!ProjectIsSaved) {

			SavePromptResult savePromptResult = SavePrompter.PromptSave();

			switch (savePromptResult) {

				case SavePromptResult.CancelOperation:
					return;

				case SavePromptResult.SaveAndContinue:
					Saver.Save(GameEditingData);
					break;

				case SavePromptResult.ContinueWithoutSaving:
					break;

				default:
					throw new InvalidEnumArgumentException();
			}
		}

		Result<GameEditingData, IGameSaver.OpenError> openResult = Saver.Open();

		switch (openResult.Resolve()) {

			case GameEditingData newGameEditingData:
				GameEditingData = newGameEditingData;
				return;

			case IGameSaver.OpenError:
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	public static void NewGameProject() {
		
		if (!ProjectIsSaved) {
			
			SavePromptResult savePromptResult = SavePrompter.PromptSave();

			if (savePromptResult == SavePromptResult.CancelOperation) {
				return;
			}
			
			if (savePromptResult == SavePromptResult.SaveAndContinue) {
				Saver.Save(GameEditingData);
			}
		}

		GameEditingData = DefaultEditingDataValues.DefaultEditingData;
	}

}