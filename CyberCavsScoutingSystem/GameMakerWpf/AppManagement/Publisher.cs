using System.Diagnostics;
using System.IO;
using CCSSDomain.GameSpecification;
using CCSSDomain.Serialization;
using GameMakerWpf.Domain.Editors;
using Microsoft.Win32;
using Newtonsoft.Json;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Results;
using static GameMakerWpf.AppManagement.IPublisher;

namespace GameMakerWpf.AppManagement;



public interface IPublisher {

	public IPublishResult Publish(GameEditor gameEditor);

	public interface IPublishResult : IResult {

		public class Success : IResult.Success, IPublishResult { }

		public class Aborted : Error, IPublishResult { }

		public class GameEditorCouldNotBeConvertedToGameSpecification : Error, IPublishResult { }

		public class GameSpecificationCouldNotBeConvertedToSaveData : Error, IPublishResult { }

		public class SaveLocationDoesNotExist : Error, IPublishResult { }

		public class SaveLocationCouldNotBeWrittenTo : Error, IPublishResult { }

	}

}



public class Publisher : IPublisher {

	private static SaveFileDialog SaveFileDialog => new() {
		Title = "Select a file name and location for the published Game Specification.",
		Filter = "CCSS Game Specification (*.cgs)|*.cgs"
	};

	public IPublishResult Publish(GameEditor gameEditor) {

		IEditorToGameSpecificationResult<GameSpec> result = gameEditor.ToGameSpecification();
		GameSpec gameSpecSpecification;

		switch (result) {

			case IEditorToGameSpecificationResult<GameSpec>.Success success:
				gameSpecSpecification = success.Value;
				break;

			case IEditorToGameSpecificationResult<GameSpec>.Error error:
				return new IPublishResult.GameEditorCouldNotBeConvertedToGameSpecification {
					InnerError = ((Error)error).Optionalize()
				};

			default:
				throw new UnreachableException();
		}

		SaveFileDialog saveFileDialog = SaveFileDialog;

		bool? proceed = saveFileDialog.ShowDialog();

		if (proceed is null or false) {
			return new IPublishResult.Aborted();
		}

		string filePath = saveFileDialog.FileName;
		string[] filePathPieces = filePath.Split("\\");
		string folderPath = string.Join("\\", filePathPieces[..^1]);

		if (!Directory.Exists(folderPath)) {
			return new IPublishResult.SaveLocationDoesNotExist();
		}

		string serializedGameSpecification;
		try {
			serializedGameSpecification = JsonConvert.SerializeObject(gameSpecSpecification, JsonSettings.JsonSerializerSettings);

		} catch {
			return new IPublishResult.GameSpecificationCouldNotBeConvertedToSaveData();
		}

		try {
			File.WriteAllText(filePath, serializedGameSpecification);

		} catch {

			return new IPublishResult.SaveLocationCouldNotBeWrittenTo();
		}

		return new IPublishResult.Success();
	}

}