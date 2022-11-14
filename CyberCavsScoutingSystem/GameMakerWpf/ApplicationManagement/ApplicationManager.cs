using System;
using System.Collections.Generic;
using System.ComponentModel;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Views;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using static GameMakerWpf.ApplicationManagement.ISavePrompter;

namespace GameMakerWpf.ApplicationManagement;



public static class ApplicationManager {

	private static GameEditor _GameEditor = new(DefaultEditingDataValues.DefaultGameEditingData);
	public static GameEditor GameEditor {

		get => _GameEditor;

		private set {
			value.AnythingChanged.Subscribe(MarkProjectUnsaved);
			_GameEditor = value;
			ProjectIsSaved = false;
			InvokeGameProjectChangeActions();
		}
	}

	private static readonly List<Action> GameProjectChangeActions = new();

	private static IGameMakerMainView MainView { get; } = new MainWindow();
	private static ISaver Saver { get; } = new Saver();
	private static ISavePrompter SavePrompter => new SavePrompter();
	private static IErrorPresenter ErrorPresenter { get; } = new ErrorPresenter();

	private static bool ProjectIsSaved { get; set; } = true;



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

			case SavePromptResult.Cancel:
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

		Result<ISaver.SaveError> result = Saver.Save(GameEditor.ToEditingData());

		switch (result.Resolve()) {
			
			case Success:
				ProjectIsSaved = true;
				return;

			case ISaver.SaveError { ErrorType: ISaver.SaveError.Types.NoSaveLocationSpecified }:
				ErrorPresenter.DisplayError(ErrorData.SaveError.NoSaveLocationSpecifiedCaption, ErrorData.SaveError.NoSaveLocationSpecifiedMessage);
				return;

			case ISaver.SaveError { ErrorType: ISaver.SaveError.Types.SerializationFailed }:
				ErrorPresenter.DisplayError(ErrorData.SaveError.SerializationFailedCaption, ErrorData.SaveError.SerializationFailedMessage);
				return;

			case ISaver.SaveError { ErrorType: ISaver.SaveError.Types.SaveLocationInaccessible }:
				ErrorPresenter.DisplayError(ErrorData.SaveError.SaveLocationInaccessibleCaption, ErrorData.SaveError.SaveLocationInaccessibleMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}
	}

	public static void SaveGameProjectAs() {

		Result<ISaver.SetSaveLocationError> result = Saver.SetSaveLocation();

		switch (result.Resolve()) {
			
			case Success:
				break;

			case ISaver.SetSaveLocationError { ErrorType: ISaver.SetSaveLocationError.Types.Aborted }:
				return;

			case ISaver.SetSaveLocationError { ErrorType: ISaver.SetSaveLocationError.Types.SaveLocationIsInvalid }:
				ErrorPresenter.DisplayError(ErrorData.SaveAsError.SaveLocationIsInvalidCaption, ErrorData.SaveAsError.SaveLocationIsInvalidMessage);
				return;

			default:
				throw new ShouldMatchOtherCaseException();
		}

		SaveGameProject();
	}

	public static void OpenGameProject() {

		PromptIfUnsaved(out bool cancelOperation);
		if (cancelOperation) {
			return;
		}

		Result<GameEditingData, ISaver.OpenError> openResult = Saver.Open();

		switch (openResult.Resolve()) {

			case Success<GameEditingData> newGameEditingData:
				GameEditor = new(newGameEditingData.Value);
				return;

			case ISaver.OpenError { ErrorType: ISaver.OpenError.Types.Aborted }:
				return;
				
			case ISaver.OpenError { ErrorType: ISaver.OpenError.Types.SaveLocationInaccessible }:
				ErrorPresenter.DisplayError(ErrorData.OpenError.SaveLocationInaccessibleCaption, ErrorData.OpenError.SaveLocationInaccessibleMessage);
				return;

			case ISaver.OpenError { ErrorType: ISaver.OpenError.Types.SavedDataCouldNotBeConverted }:
				ErrorPresenter.DisplayError(ErrorData.OpenError.SavedDataCouldNotBeConvertedCaption, ErrorData.OpenError.SavedDataCouldNotBeConvertedMessage);
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

		GameEditor = new(DefaultEditingDataValues.DefaultGameEditingData);
		ProjectIsSaved = true;
	}

}