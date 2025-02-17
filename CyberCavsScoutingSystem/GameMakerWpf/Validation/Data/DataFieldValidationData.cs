using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Conversion;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data; 



public static class DataFieldValidationData {

	public static class Name {

		public static class Length {

			public const int LowerErrorThreshold = 0;

			public const int UpperErrorThreshold = 1000;
			public const int UpperWarningThreshold = 30;
			public const int UpperAdvisoryThreshold = 20;



			public static readonly Error TooShortError =
				new("Empty Name", ErrorSeverity.Error, "An data field cannot have a blank name.");

			public static readonly Error TooLongError
				= new("Long Name", ErrorSeverity.Error, $"An data field name cannot be more than {UpperErrorThreshold} characters.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The data field name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Advisory, "The data field name is rather long.");

		}

		public static Error GetDuplicateNameError(string name) {

			return new("Duplicate Name", ErrorSeverity.Error, $"Multiple data fields have the name {name}.");
		}

	}

	public static class Option {

		public const int LowerErrorThreshold = 0;

		public const int UpperErrorThreshold = 1000;
		public const int UpperWarningThreshold = 30;
		public const int UpperAdvisoryThreshold = 20;



		public static readonly Error TooShortError =
			new("Empty Name", ErrorSeverity.Error, "An option cannot have a blank name.");

		public static readonly Error TooLongError
			= new("Long Name", ErrorSeverity.Error, $"An option name cannot be more than {UpperErrorThreshold} characters.");

		public static readonly Error TooLongWarning
			= new("Long Name", ErrorSeverity.Warning, "The option name is alarmingly long");

		public static readonly Error TooLongAdvisory
			= new("Long Name", ErrorSeverity.Advisory, "The option name is rather long.");

	}

	public static class IntegerValue {

		public static string EntryInstruction = $"Please enter an integer between {int.MinValue} and {int.MaxValue}.";
		public static string EnteredText = $"An integer between {int.MinValue} and {int.MaxValue}.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"An integer value must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooNegativeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too negative to be converted to" +
			                                                   $" an integer value. The maximum possible value is {int.MinValue}.");
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to" +
			                                                   $" an integer value. The maximum possible value is {int.MaxValue}.");
		}

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"A color value must be a whole number.");

		private static readonly Error MinusSignMustBeAtStartError = new("Minus Sign Must be at Start", ErrorSeverity.Error);

		public static readonly IntegerConversionErrorSet ConversionErrorSet = new() {
			RequiresValueError = RequiresValueError,
			InvalidCharactersErrorGetter = GetInvalidCharactersError,
			ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
			ValueTooNegativeErrorGetter = ValueTooNegativeErrorGetter,
			MustBeIntegerError = MustBeIntegerError,
			MinusSignMustBeAtStartError = MinusSignMustBeAtStartError
		};

	}
}