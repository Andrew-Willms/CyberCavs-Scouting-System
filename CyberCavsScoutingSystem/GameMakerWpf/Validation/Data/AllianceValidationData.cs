using CCSSDomain;
using GameMakerWpf.Validation.Conversion;
using UtilitiesLibrary;
using UtilitiesLibrary.Extensions;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Data; 



public static class AllianceValidationData {

	public static class Name {

		public static class Length {

			public const int LowerErrorThreshold = 0;
			public const int LowerWarningThreshold = 2;
			public const int LowerAdvisoryThreshold = 4;

			public const int UpperErrorThreshold = 1000;
			public const int UpperWarningThreshold = 30;
			public const int UpperAdvisoryThreshold = 20;



			public static readonly Error TooShortError =
				new("Empty Name", ErrorSeverity.Error, "An alliance cannot have a blank name.");

			public static readonly Error TooShortWarning =
				new("Short Name", ErrorSeverity.Warning, "The alliance name is alarmingly short.");

			public static readonly Error TooShortAdvisory =
				new("Short Name", ErrorSeverity.Advisory, "The alliance name is rather short.");



			public static readonly Error TooLongError
				= new("Long Name", ErrorSeverity.Error, $"An alliance name cannot be more than {UpperErrorThreshold} characters.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The alliance name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Advisory, "The alliance name is rather long.");

		}

		public static Error GetDuplicateNameError(string name) {

			return new("Duplicate Name", ErrorSeverity.Error, $"Multiple alliances have the name {name}.");
		}

		public const string ShouldEndWith = " Alliance";

		public static readonly Error DoesNotEndWithCorrectSequenceError = new("Does Not End With \"Alliance\"", ErrorSeverity.Advisory,
				"Typically alliance names should follow the format \"{Colour} Alliance\".");

	}

	public static class Color {

		public static class Component {

			public const string EntryInstruction = "Please enter an integer between 0 and 255.";
			public const string EnteredText = "An integer between 0 and 255.";

			private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
				"A color value must be specified.");

			private static Error GetInvalidCharactersError(char[] invalidCharacters) {
				return CommonValidationData.GetInvalidCharactersError(invalidCharacters);
			}

			private static Error ValueTooLargeErrorGetter(string givenValue) {

				return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to" +
				                                                   $" a color value. The maximum possible value is {byte.MaxValue}.");
			}

			private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
				"A color value cannot be a negative number.");

			private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
				"A color value must be a whole number.");

			public static readonly WholeConversionErrorSet ConversionErrorSet = new() {
				RequiresValueError = RequiresValueError,
				InvalidCharactersErrorGetter = GetInvalidCharactersError,
				ValueTooLargeErrorGetter = ValueTooLargeErrorGetter,
				CannotBeNegativeError = CannotBeNegativeError,
				MustBeIntegerError = MustBeIntegerError
			};

		}

		private const int SimilarityErrorThreshold = 0;
		private const int SimilarityWarningThreshold = 10;
		private const int SimilarityAdvisoryThreshold = 20;

		public static Optional<Error> GetColorSimilarityError(int colorDifference, string otherAllianceName) {

			return colorDifference switch {
				<= SimilarityErrorThreshold => new Error("Identical Color", ErrorSeverity.Error,
					$"The alliance color is identical to that of the {otherAllianceName}").Optionalize(),

				<= SimilarityWarningThreshold => new Error("Similar Color", ErrorSeverity.Warning,
					$"The alliance color is very similar to that of the {otherAllianceName}").Optionalize(),

				<= SimilarityAdvisoryThreshold => new Error("Similar Color", ErrorSeverity.Advisory,
					$"The alliance color is similar to that of the {otherAllianceName}").Optionalize(),

				_ => Optional.NoValue
			};
		}

	}

}