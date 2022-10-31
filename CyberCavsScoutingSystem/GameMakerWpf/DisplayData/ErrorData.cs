namespace GameMakerWpf.DisplayData; 



public class ErrorData {

	public class SaveError {

		public static string NoSaveLocationSpecifiedCaption => "No Save Location";
		public static string NoSaveLocationSpecifiedMessage =>
			"The Game Project does not have a specified save location. Try using \"SetSaveLocation\".";

		public static string SerializationFailedCaption => "Could Not Be Converted to Saving Format";
		public static string SerializationFailedMessage =>
			"The Game Project could not be converted into the format it is saved in. " +
			"Try changing values in the Game Project and saving again. " +
			"Please contact the creators of the CCSS if you receive this error.";

		public static string SaveLocationInaccessibleCaption => "Save Location Inaccessible";
		public static string SaveLocationInaccessibleMessage =>
			"The save location specified could not be written to." +
			"Verify that this resource exists and your user account has permission to write to it. " +
			"Alternately, try specifying a different save location using \"SetSaveLocation\".";
	}

	public class SaveAsError {

		public static string SaveLocationIsInvalidCaption => "Save Location Invalid";
		public static string SaveLocationIsInvalidMessage =>
			"The save location specified is invalid. Verify that the location you specified exists. " +
			"Alternately, try specifying a different save location using \"SetSaveLocation\".";
	}

	public class OpenError {

		public static string SaveLocationInaccessibleCaption => "Save Location Inaccessible";
		public static string SaveLocationInaccessibleMessage =>
			"The save location specified could nto be accessed. Verify that the location you specified exists. " +
			"Alternately, try opening a different GameProject using \"Open\".";

		public static string SavedDataCouldNotBeConvertedCaption => "Saved Data Could not Be Converted";
		public static string SavedDataCouldNotBeConvertedMessage =>
			"The saved data could not be converted into a GameProject. Verify that you specified the correct save location. " +
			"Alternately, try opening a different GameProject using \"Open\". " +
			"Please contact the creators of the CCSS if you receive this error.";
	}

}