using System.ComponentModel;
using System.Diagnostics;
using GameMakerWpf.DisplayData.Errors.ErrorData.AppManagerErrors;
using GameMakerWpf.Domain.Data;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Views;
using UtilitiesLibrary.WPF;
using static GameMakerWpf.AppManagement.ISavePrompter;
using static GameMakerWpf.AppManagement.ISaver.ISaveAsResult;
using static GameMakerWpf.AppManagement.ISaver.ISaveResult;

namespace GameMakerWpf.AppManagement;



public abstract class AppManagerDependent : DependentControl<AppManager> {

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

	private IGameMakerMainView MainView { get; set; } = null!; // must be initialized after the AppManager is finished construction
	private static IErrorPresenter ErrorPresenter => new ErrorPresenter(); // stateless
	private readonly ISaver Saver = new Saver(); // stateful
	private static ISavePrompter SavePrompter => new SavePrompter(); // single use
	private readonly IPublisher Publisher = new Publisher(); // stateless

	private bool ProjectIsSaved { get; set; } = true;


	public AppManager() {

		GameEditor = new(DefaultEditingDataValues.DefaultGameEditingData);
	}

	public void ApplicationStartup() {

		MainView = new MainWindow();

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

		ISaver.ISaveResult result = Saver.Save(GameEditor.ToEditingData());

		switch (result) {

			case ISaver.ISaveResult.Success:
				ProjectIsSaved = true;
				return;

			case NoSaveLocationSpecified error:
				ErrorPresenter.DisplayError(error, SaveErrors.NoSaveLocationSpecified);
				break;

			case SaveLocationInaccessible error:
				ErrorPresenter.DisplayError(error, SaveErrors.SaveLocationInaccessible);
				break;

			case GameEditingDataCouldNotBeConvertedToSaveData error:
				ErrorPresenter.DisplayError(error, SaveErrors.GameEditingDataCouldNotBeConvertedToSaveData);
				break;

			default:
				throw new UnreachableException();
		}
	}

	public void SaveGameProjectAs() {

		ISaver.ISaveAsResult result = Saver.SetSaveLocation();

		switch (result) {

			case UtilitiesLibrary.Results.Success:
				break;

			case Aborted:
				return;

			case SaveLocationIsInvalid error:
				ErrorPresenter.DisplayError(error, SaveAsErrors.SaveLocationIsInvalid);
				return;

			default:
				throw new UnreachableException();
		}

		SaveGameProject();
	}

	public void OpenGameProject() {

		PromptIfUnsaved(out bool cancelOperation);
		if (cancelOperation) {
			return;
		}

		ISaver.IOpenResult openResult = Saver.Open();

		switch (openResult) {

			case ISaver.IOpenResult.Success newGameEditingData:
				GameEditor = new(newGameEditingData.Value);
				return;

			case ISaver.IOpenResult.Aborted:
				return;

			case ISaver.IOpenResult.SaveLocationInaccessible error:
				ErrorPresenter.DisplayError(error, OpenError.SaveLocationInaccessible);
				return;

			case ISaver.IOpenResult.SavedDataCouldNotBeConvertedToGameEditingData error:
				ErrorPresenter.DisplayError(error, OpenError.SavedDataCouldNotBeConvertedToGameEditingData);
				return;

			default:
				throw new UnreachableException();
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

	public void Publish() {

		IPublisher.IPublishResult result = Publisher.Publish(GameEditor);

		switch (result) {

			case UtilitiesLibrary.Results.Success:
				break;

			case IPublisher.IPublishResult.Aborted:
				return;

			case IPublisher.IPublishResult.GameEditorCouldNotBeConvertedToGameSpecification error:
				ErrorPresenter.DisplayError(error, PublishErrors.GameEditorCouldNotBeConvertedToGameSpecification);
				return;

			case IPublisher.IPublishResult.GameSpecificationCouldNotBeConvertedToSaveData error:
				ErrorPresenter.DisplayError(error, PublishErrors.GameSpecificationCouldNotBeConvertedToSaveData);
				return;

			case IPublisher.IPublishResult.SaveLocationDoesNotExist error:
				ErrorPresenter.DisplayError(error, PublishErrors.SaveLocationDoesNotExist);
				return;

			case IPublisher.IPublishResult.SaveLocationCouldNotBeWrittenTo error:
				ErrorPresenter.DisplayError(error, PublishErrors.SaveLocationCouldNotBeWrittenTo);
				return;

			default:
				throw new UnreachableException();
		}

	}



	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new(propertyName));
	}

}