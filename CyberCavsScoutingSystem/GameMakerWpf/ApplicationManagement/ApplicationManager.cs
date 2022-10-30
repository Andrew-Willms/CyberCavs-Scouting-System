using System;
using System.Collections.Generic;
using System.ComponentModel;
using GameMakerWpf.Domain;
using GameMakerWpf.Domain.DomainData;
using GameMakerWpf.Views;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using static GameMakerWpf.ApplicationManagement.ISavePrompter;

namespace GameMakerWpf.ApplicationManagement;



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

	private static ISaver Saver { get; } = new Saver();

	private static ISavePrompter SavePrompter { get; } = new SavePrompter();

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



	private static void PromptIfUnsaved(out bool cancelOperation) {

		if (ProjectIsSaved) {
			cancelOperation = false;
			return;
		}

		SavePromptResult savePromptResult = SavePrompter.PromptSave();

		switch (savePromptResult) {

			case SavePromptResult.CancelOperation:
				cancelOperation = true;
				return;

			case SavePromptResult.SaveAndContinue:
				SaveGameProject();
				cancelOperation = false;
				return;

			case SavePromptResult.ContinueWithoutSaving:
				cancelOperation = false;
				return;

			default:
				throw new InvalidEnumArgumentException();
		}
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

		PromptIfUnsaved(out bool cancelOperation);
		if (cancelOperation) {
			return;
		}

		Result<GameEditingData, ISaver.OpenError> openResult = Saver.Open();

		switch (openResult.Resolve()) {

			case GameEditingData newGameEditingData:
				GameEditingData = newGameEditingData;
				return;

			case ISaver.OpenError:
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	public static void NewGameProject() {

		PromptIfUnsaved(out bool cancelOperation);
		if (cancelOperation) {
			return;
		}

		GameEditingData = DefaultEditingDataValues.DefaultEditingData;
	}

}