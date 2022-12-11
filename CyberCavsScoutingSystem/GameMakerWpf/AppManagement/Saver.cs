using System.Data;
using System.IO;
using GameMakerWpf.Domain.EditingData;
using Microsoft.Win32;
using Newtonsoft.Json;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Results;

namespace GameMakerWpf.AppManagement;



public interface ISaver {

	public class SaveError : Error<SaveError.Types> {

		public enum Types {
			NoSaveLocationSpecified,
			GameEditingDataCouldNotBeConvertedToSaveData,
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
			SavedDataCouldNotBeConvertedToGameEditingData
		}

	}



	public bool ProjectHasSaveLocation { get; }

	public Result<SaveError> Save(GameEditingData gameEditingData);

	public Result<SetSaveLocationError> SetSaveLocation();

	public Result<GameEditingData, OpenError> Open();

}



public class Saver : ISaver {

	public bool ProjectHasSaveLocation => FilePath.HasValue;

	private Optional<string> FilePath = Optional.NoValue;

	public Result<ISaver.SaveError> Save(GameEditingData gameEditingData) {

		if (!FilePath.HasValue) {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.NoSaveLocationSpecified
			};
		}

		string serializedProject;
		try {
			serializedProject = JsonConvert.SerializeObject(gameEditingData, JsonSerializerSettings);

		} catch {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.GameEditingDataCouldNotBeConvertedToSaveData
			};
		}

		try {
			File.WriteAllText(FilePath.Value, serializedProject);

		} catch {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.SaveLocationInaccessible
			};
		}

		return new Success();
	}

	public Result<ISaver.SetSaveLocationError> SetSaveLocation() {

		SaveFileDialog saveFileDialog = SaveFileDialog;

		bool? proceed = saveFileDialog.ShowDialog();

		if (proceed is null or false) {
			return new ISaver.SetSaveLocationError { ErrorType = ISaver.SetSaveLocationError.Types.Aborted };
		}

		string filePath = saveFileDialog.FileName;
		string[] filePathPieces = filePath.Split("\\");
		string folderPath = string.Join("\\", filePathPieces[..^1]);

		if (!Directory.Exists(folderPath)) {
			return new ISaver.SetSaveLocationError { ErrorType = ISaver.SetSaveLocationError.Types.SaveLocationIsInvalid };
		}

		FilePath = filePath.Optionalize();

		return new Success();
	}

	public Result<GameEditingData, ISaver.OpenError> Open() {

		OpenFileDialog openFileDialog = OpenFileDialog;

		bool? proceed = openFileDialog.ShowDialog();

		if (proceed is null or false) {
			return new ISaver.OpenError { ErrorType = ISaver.OpenError.Types.Aborted };
		}

		string filePath = openFileDialog.FileName;
		string serializedGameEditingData;

		try {
			serializedGameEditingData = File.ReadAllText(filePath);

		} catch {
			return new ISaver.OpenError { ErrorType = ISaver.OpenError.Types.SaveLocationInaccessible };
		}

		try {
			GameEditingData newGameEditingData =
				JsonConvert.DeserializeObject<GameEditingData>(serializedGameEditingData, JsonSerializerSettings)
				?? throw new NoNullAllowedException();

			FilePath = filePath.Optionalize();
			return newGameEditingData;

		} catch {
			return new ISaver.OpenError { ErrorType = ISaver.OpenError.Types.SavedDataCouldNotBeConvertedToGameEditingData };
		}
	}



	private static readonly JsonSerializerSettings JsonSerializerSettings = new() {
		TypeNameHandling = TypeNameHandling.All,
		Formatting = Formatting.Indented
	};

	private static OpenFileDialog OpenFileDialog => new() {
		Title = "Select a file to open.",
		Filter = "CCSS Game Project (*.cgp)|*.cgp"
	};

	private static SaveFileDialog SaveFileDialog => new() {
		Title = "Select a file name and location for the project to be saved.",
		Filter = "CCSS Game Project (*.cgp)|*.cgp"
	};

}