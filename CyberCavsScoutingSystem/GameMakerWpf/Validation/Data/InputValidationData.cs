using CCSSDomain;
using GameMakerWpf.Domain;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;


namespace GameMakerWpf.Validation.Data; 



public static class InputValidationData {

	public static class DataField {

		public static readonly Error DataFieldDoesNotExistError =
			new("Data Field Does not Exist", ErrorSeverity.Error, "An Integer type DataField of the given name does not exist.");

	}

	public static class InputTextLength {

		public const int UpperErrorThreshold = 1000;
		public const int UpperWarningThreshold = 30;
		public const int UpperAdvisoryThreshold = 20;



		public static readonly Error TooLongError
			= new("Long Button Text", ErrorSeverity.Error, $"Button text cannot be more than {UpperErrorThreshold} characters.");

		public static readonly Error TooLongWarning
			= new("Long Button Text", ErrorSeverity.Warning, "The Button text is alarmingly long");

		public static readonly Error TooLongAdvisory
			= new("Long Button Text", ErrorSeverity.Advisory, "The Button text is rather long.");

	}

}