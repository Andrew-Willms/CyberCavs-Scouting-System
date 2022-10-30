using System.Data;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using GameMakerWpf.Domain;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary;

namespace GameMakerWpf.ApplicationManagement; 



public class Saver : ISaver {

	public bool ProjectHasSaveLocation => FilePath.HasValue;

	private Optional<string> FilePath = Optional.NoValue;

	public Result<ISaver.SaveError> Save(GameEditingData gameEditingData) {

		if (!FilePath.HasValue) {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.ProjectHasNoSaveLocation
			};
		}

		string serializedProject;
		try {
			serializedProject = JsonSerializer.Serialize(gameEditingData, SavingSerializerDefaults);

		} catch {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.ProjectCouldNotBeSerialized
			};
		}

		try {
			File.WriteAllText(FilePath.Value, serializedProject);

		} catch {

			return new ISaver.SaveError {
				ErrorType = ISaver.SaveError.Types.SaveLocationCouldNotBeWrittenTo
			};
		}

		return new Success();
	}

	public Result<ISaver.SaveAsError> SaveAs(GameEditingData gameEditingData) {

		SaveFileDialog saveFileDialog = SaveFileDialog;

		bool? proceed = saveFileDialog.ShowDialog();

		if (proceed is null or false) {
			return new ISaver.SaveAsError { ErrorType = ISaver.SaveAsError.Types.Aborted };
		}

		string filePath = saveFileDialog.FileName;
		string[] filePathPieces = filePath.Split("\\");
		string folderPath = string.Join("\\", filePathPieces[..^1]);

		if (!Directory.Exists(folderPath)) {
			return new ISaver.SaveAsError { ErrorType = ISaver.SaveAsError.Types.SaveLocationIsInvalid };
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
			return new ISaver.OpenError { ErrorType = ISaver.OpenError.Types.SaveLocationCouldNotBeRead };
		}

		try {
			GameEditingData? newGameEditingData = JsonSerializer.Deserialize<GameEditingData>(serializedGameEditingData);
			return newGameEditingData ?? throw new NoNullAllowedException();

		} catch {
			return new ISaver.OpenError { ErrorType = ISaver.OpenError.Types.SavedDataCouldNotBeConverted };
		}
	}



	private static readonly JsonSerializerOptions SavingSerializerDefaults = new() {
		WriteIndented = true,
	};

	private static readonly OpenFileDialog OpenFileDialog = new() {
		Title = "Select a file to open.",
		Filter = "CCSS Game Project (*.cgp)|*.cgp"
	};

	private static readonly SaveFileDialog SaveFileDialog = new() {
		Title = "Select a file name and location for the project to be saved.",
		Filter = "CCSS Game Project (*.cgp)|*.cgp"
	};

}