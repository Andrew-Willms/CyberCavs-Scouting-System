using System.Diagnostics;
using System.IO;
using CCSSDomain.GameSpecification;
using GameMakerWpf.Domain.Editors;
using Microsoft.Win32;
using Newtonsoft.Json;
using UtilitiesLibrary.Results;

namespace GameMakerWpf.AppManagement;



public interface IPublisher {

	public Result<PublishingError> Publish(GameEditor gameEditor);

	public class PublishingError : Error<PublishingError.Types> {

		public enum Types {
			Aborted,
			GameEditorCouldNotBeConvertedToGameSpecification,
			GameSpecificationCouldNotBeConvertedToSaveData,
			SaveLocationDoesNotExist,
			SaveLocationCouldNotBeWrittenTo
		}

	}

}



public class Publisher : IPublisher {

	private static readonly JsonSerializerSettings JsonSerializerSettings = new() {
		TypeNameHandling = TypeNameHandling.All,
		Formatting = Formatting.Indented
	};

	private static SaveFileDialog SaveFileDialog => new() {
		Title = "Select a file name and location for the published Game Specification.",
		Filter = "CCSS Game Specification (*.cgs)|*.cgs"
	};



	public Result<IPublisher.PublishingError> Publish(GameEditor gameEditor) {

		Result<Game, EditorToGameSpecificationError> result = gameEditor.ToGameSpecification();
		Game gameSpecification;

		switch (result.Resolve()) {

			case Success<Game> success:
				gameSpecification = success.Value;
				break;

			case EditorToGameSpecificationError { ErrorType: EditorToGameSpecificationError.Types.EditorIsInvalid }:
				return new IPublisher.PublishingError {
					ErrorType = IPublisher.PublishingError.Types.GameEditorCouldNotBeConvertedToGameSpecification
				};

			default:
				throw new UnreachableException();
		}

		SaveFileDialog saveFileDialog = SaveFileDialog;

		bool? proceed = saveFileDialog.ShowDialog();

		if (proceed is null or false) {
			return new IPublisher.PublishingError { ErrorType = IPublisher.PublishingError.Types.Aborted };
		}

		string filePath = saveFileDialog.FileName;
		string[] filePathPieces = filePath.Split("\\");
		string folderPath = string.Join("\\", filePathPieces[..^1]);

		if (!Directory.Exists(folderPath)) {
			return new IPublisher.PublishingError { ErrorType = IPublisher.PublishingError.Types.SaveLocationDoesNotExist };
		}

		string serializedGameSpecification;
		try {
			serializedGameSpecification = JsonConvert.SerializeObject(gameSpecification, JsonSerializerSettings);

		} catch {

			return new IPublisher.PublishingError {
				ErrorType = IPublisher.PublishingError.Types.GameSpecificationCouldNotBeConvertedToSaveData
			};
		}

		try {
			File.WriteAllText(filePath, serializedGameSpecification);

		} catch {

			return new IPublisher.PublishingError {
				ErrorType = IPublisher.PublishingError.Types.SaveLocationCouldNotBeWrittenTo
			};
		}

		return new Success();
	}



}