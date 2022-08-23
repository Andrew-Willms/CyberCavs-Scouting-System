using CCSSDomain;
using GameMakerWpf.Validation.Conversion;
using UtilitiesLibrary.Extensions;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data; 



public static class GameValidationData {

	public static class Year {

		public const string EntryInstruction = "Please enter an integer.";
		public const string EnteredText = "An integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error, "A year must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to" +
			                                                   $" a year. The maximum possible value is {int.MaxValue}.");
		}

		private static Error ValueTooNegativeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too negative to be converted to" +
			                                                   $" a year. The lowest possible value is {int.MinValue}.");
		}

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"The year must be a whole number.");

		public static readonly IntegerConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			ValueTooSmallErrorGetter = ValueTooNegativeErrorGetter,
			MustBeIntegerError = MustBeIntegerError
		};



		public const int FirstYearOfFirst = 1992;
		public const int FutureYearAdvisoryThreshold = 1;
		public const int FutureYearWarningThreshold = 10;

		public static readonly Error NegativeYearWarning = new("Negative Year", ErrorSeverity.Warning,
			"A negative year does not have a clear meaning.");

		public static readonly Error YearPredatesFirstWarning = new("Year Predates FIRST", ErrorSeverity.Warning,
			"The year specified is before the year of the first FRC event.");

		public static readonly Error FutureYearAdvisory = new("Future Year", ErrorSeverity.Advisory,
			$"The year specified is more than {FutureYearAdvisoryThreshold.ToWrittenConvention()} year in the future.");

		public static readonly Error FutureYearWarning = new("Future Year", ErrorSeverity.Warning,
			$"The year specified is more than {FutureYearWarningThreshold.ToWrittenConvention()} year in the future.");

	}

	public static class RobotsPerAlliance {

		public const string EntryInstruction = "Please enter a positive integer.";
		public const string EnteredText = "A positive integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"The number of robots per alliance must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a number of " +
			                                                   $"robots per alliance. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"The number of robots per alliance cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"The number of robots per alliance must be a whole number.");

		public static readonly WholeConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			CannotBeNegativeError = CannotBeNegativeError,
			MustBeIntegerError = MustBeIntegerError
		};

	}

	public static class AlliancesPerMatch {

		public const string EntryInstruction = "Please enter a positive integer.";
		public const string EnteredText = "A positive integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"The number of alliances per match must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a number of " +
			                                                   $"alliances per match. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"The number of alliances per match cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"The number of alliances per match must be a whole number.");

		public static readonly WholeConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			CannotBeNegativeError = CannotBeNegativeError,
			MustBeIntegerError = MustBeIntegerError
		};

	}

	public static class Name {

		public static class Length {

			public const int LowerErrorThreshold = 0;
			public const int LowerWarningThreshold = 2;
			public const int LowerAdvisoryThreshold = 4;

			public const int UpperErrorThreshold = 1000;
			public const int UpperWarningThreshold = 40;
			public const int UpperAdvisoryThreshold = 30;



			public static readonly Error TooShortError =
				new("Empty Name", ErrorSeverity.Error, "A Game cannot have a blank name.");

			public static readonly Error TooShortWarning =
				new("Short Name", ErrorSeverity.Warning, "The game name is alarmingly short.");

			public static readonly Error TooShortAdvisory =
				new("Short Name", ErrorSeverity.Advisory, "The game name is rather short.");



			public static readonly Error TooLongError
				= new("Long Name", ErrorSeverity.Error, $"A game name cannot be more than {UpperErrorThreshold} characters.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The game name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Advisory, "The game name is rather long.");

		}

	}

}