using GameMakerWpf.Domain;
using UtilitiesLibrary;

namespace GameMakerWpf.ApplicationManagement;



public interface IGameMakerMainView {

	public void Show();
}



public interface ISavePrompter {

	public class SavePromptError : Error<SavePromptResult> { }

	public enum SavePromptResult {
		SaveAndContinue,
		ContinueWithoutSaving,
		CancelOperation
	}

	public SavePromptResult PromptSave();

}



public interface ISaver {

	public class SaveError : Error<SaveError.Types> {

		public enum Types {
			ProjectHasNoSaveLocation,
			ProjectCouldNotBeSerialized,
			SaveLocationCouldNotBeWrittenTo
		}

	}

	public class SaveAsError : Error<SaveAsError.Types> {

		public enum Types {
			Aborted,
			SaveLocationIsInvalid
		}

	}

	public class OpenError : Error<OpenError.Types> {

		public enum Types {
			Aborted,
			SaveLocationCouldNotBeRead,
			SavedDataCouldNotBeConverted
		}

	}



	public bool ProjectHasSaveLocation { get; }

	public Result<SaveError> Save(GameEditingData gameEditingData);

	public Result<SaveAsError> SaveAs(GameEditingData gameEditingData);

	public Result<GameEditingData, OpenError> Open();

}