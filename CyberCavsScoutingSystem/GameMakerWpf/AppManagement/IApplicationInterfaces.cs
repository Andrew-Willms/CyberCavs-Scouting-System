using GameMakerWpf.Domain.EditingData;
using UtilitiesLibrary;

namespace GameMakerWpf.AppManagement;



public interface IGameMakerMainView {

	public void Show();

}



public interface IErrorPresenter {

	public void DisplayError(string caption, string message);

}



public interface ISavePrompter {

	public enum SavePromptResult {
		SaveAndContinue,
		ContinueWithoutSaving,
		Cancel
	}

	public SavePromptResult PromptSave();

}



public interface ISaver {

	public class SaveError : Error<SaveError.Types> {

		public enum Types {
			NoSaveLocationSpecified,
			SerializationFailed,
			SaveLocationInaccessible
		}

	}

	public class SetSaveLocationError : Error<SetSaveLocationError.Types> {

		public enum Types {
			Aborted,
			SaveLocationIsInvalid
		}

	}

	public class OpenError : Error<OpenError.Types> {

		public enum Types {
			Aborted,
			SaveLocationInaccessible,
			SavedDataCouldNotBeConverted
		}

	}



	public bool ProjectHasSaveLocation { get; }

	public Result<SaveError> Save(GameEditingData gameEditingData);

	public Result<SetSaveLocationError> SetSaveLocation();

	public Result<GameEditingData, OpenError> Open();

}