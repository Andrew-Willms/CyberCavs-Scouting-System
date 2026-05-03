using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Conversion;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data; 



public static class ButtonValidationData {

	public static class DataField {

		public static readonly Error DataFieldDoesNotExistError =
			new("Data Field Does not Exist", ErrorSeverity.Error, "An Integer type DataField of the given name does not exist.");

	}

	public static class ButtonTextLength {

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

	public static class IncrementAmount {

		public const string EntryInstruction = "Please enter an integer between 0 and 255.";
		public const string EnteredText = "An integer between 0 and 255.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"A color value must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error,
				$"The given value, {givenValue}, is too large to be converted to a color value. The maximum possible value is {byte.MaxValue}.");
		}

		private static Error ValueTooNegativeErrorGetter(string givenValue) {

			return new("Value Too Negative", ErrorSeverity.Error,
				$"The given value, {givenValue}, is too negative to be converted to a color value. The maximum possible value is {int.MinValue}.");
		}

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"A color value must be a whole number.");

		private static readonly Error MinusSignMustBeAtStartError = new("Minus Sign Must be at Start", ErrorSeverity.Error,
			"A minus sign can only be the first character.");

		public static readonly IntegerConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			ValueTooNegativeErrorGetter = ValueTooNegativeErrorGetter,
			MustBeIntegerError = MustBeIntegerError,
			MinusSignMustBeAtStartError = MinusSignMustBeAtStartError
		};

	}

	public static class XPosition {

		public const string EntryInstruction = "Please enter an integer between 0 and 255.";
		public const string EnteredText = "An integer between 0 and 255.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"A color value must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error,
				$"The given value, {givenValue}, is too large to be converted to a color value. The maximum possible value is {byte.MaxValue}.");
		}

		private static Error ValueTooNegativeErrorGetter(string givenValue) {

			return new("Value Too Negative", ErrorSeverity.Error,
				$"The given value, {givenValue}, is too negative to be converted to a color value. The maximum possible value is {int.MinValue}.");
		}

		private static readonly Error MinusSignMustBeAtStartError = new("Minus Sign Must be at Start", ErrorSeverity.Error,
			"A minus sign can only be the first character.");

		private static readonly Error TooManyDecimalPointsError = new("Too Many Decimal Points", ErrorSeverity.Error,
			"There can only be one decimal point.");

		public static readonly FloatConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			ValueTooNegativeErrorGetter = ValueTooNegativeErrorGetter,
			MinusSignMustBeAtStartError = MinusSignMustBeAtStartError,
			TooManyDecimalPointsError = TooManyDecimalPointsError
		};

	}

}