using System.ComponentModel;
using GameMakerWpf.DisplayData;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Views;
using UtilitiesLibrary;
using UtilitiesLibrary.Results;
using UtilitiesLibrary.WPF;
using static GameMakerWpf.AppManagement.ISavePrompter;

namespace GameMakerWpf.AppManagement;



public abstract class AppManagerDependent: DependentControl<AppManager> {

	protected override AppManager SingletonGetter => App.Manager;

}



public class AppManager : INotifyPropertyChanged {

	private GameEditor _GameEditor = null!;
	public GameEditor GameEditor {
		get => _GameEditor;
		private set {
			value.AnythingChanged.Subscribe(() => ProjectIsSaved = false);
			_GameEditor = value;
			OnPropertyChanged(nameof(GameEditor));
		}
	}

	private DataFieldEditor? _SelectedDataField;
	public DataFieldEditor? SelectedDataField {
		get => _SelectedDataField;
		set {
			_SelectedDataField = value;
			OnPropertyChanged(nameof(SelectedDataField));
		}
	}

	private IGameMakerMainView MainView { get; set; } = null!;
	private ISaver Saver { get; set; } = null!;
	private ISavePrompter SavePrompter => new SavePrompter();
	private IErrorPresenter ErrorPresenter { get; set; } = null!;

	private bool ProjectIsSaved { get; set; } = true;


	public AppManager() {

		GameEditor = new(DefaultEditingDataValues.DefaultGameEditingData);
	}

	public void ApplicationStartup() {

		MainView = new MainWindow();
		Saver = new Saver();
		ErrorPresenter = new ErrorPresenter();

		MainView.Show();
	}



	private void PromptIfUnsaved(out bool cancelOperation) {

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

	public void SaveGameProject() {

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

	public void SaveGameProjectAs() {

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

	public void OpenGameProject() {

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

	public void NewGameProject() {

		PromptIfUnsaved(out bool cancelOperation);
		if (cancelOperation) {
			return;
		}

		GameEditor = new(DefaultEditingDataValues.DefaultGameEditingData);
		ProjectIsSaved = true;
	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}