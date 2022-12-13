using static GameMakerWpf.AppManagement.ISaver.ISaveResult;

namespace GameMakerWpf.DisplayData.Errors.ErrorData.AppManagerErrors;



public static class SaveErrors {

	public static ErrorDisplayData NoSaveLocationSpecified(NoSaveLocationSpecified error) => new() {
		Caption = "No Save Location",
		Message = "The Game Project does not have a specified save location. Try using \"SaveAs\"."
	};

	public static ErrorDisplayData GameEditingDataCouldNotBeConvertedToSaveData(GameEditingDataCouldNotBeConvertedToSaveData error) => new() {
		Caption = "Could Not Be Converted to Saving Format",
		Message = "The Game Project could not be converted into the format it is saved in. " +
		          "Try changing values in the Game Project and saving again. " +
		          "Please contact the creators of the CCSS if you receive this error."
	};

	public static ErrorDisplayData SaveLocationInaccessible(SaveLocationInaccessible error) => new() {
		Caption = "Save Location Inaccessible",
		Message = "The save location specified could not be written to." +
		          "Verify that this resource exists and your user account has permission to write to it. " +
		          "Alternately, try specifying a different save location using \"SaveAs\"."
	};

}