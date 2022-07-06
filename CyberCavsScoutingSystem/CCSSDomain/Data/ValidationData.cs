using WPFUtilities.Extensions;
using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Data;



public static class AllianceData {

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
				= new("Long Name", ErrorSeverity.Advisory, "The alliance name is rather long.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The alliance name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Error, $"An alliance name cannot be more than {UpperErrorThreshold} characters.");

		}

		public static Error GetDuplicateNameError(string name) {

			return new("Duplicate Name", ErrorSeverity.Error, $"Multiple alliances have the name {name}.");
		}

		public static readonly Error DoesNotEndWithAllianceError
			= new("Does Not End With \"Alliance\"", ErrorSeverity.Error,
				"Typically alliance names should follow the format \"{Colour} Alliance\".");

	}

	public static class Color {

		private const int SimilarityErrorThreshold = 0;
		private const int SimilarityWarningThreshold = 10;
		private const int SimilarityAdvisoryThreshold = 20;

		public static Error? GetColorSimilarityError(int colorDifference, string otherAllianceName) {

			return colorDifference switch {
				<= SimilarityErrorThreshold => new("Identical Color", ErrorSeverity.Error,
					$"The alliance color is identical to that of the {otherAllianceName}"),

				<= SimilarityWarningThreshold => new("Similar Color", ErrorSeverity.Warning,
					$"The alliance color is very similar to that of the {otherAllianceName}"),

				<= SimilarityAdvisoryThreshold => new("Similar Color", ErrorSeverity.Advisory,
					$"The alliance color is similar to that of the {otherAllianceName}"),

				_ => null
			};
		}

	}

}



public static class GameData {

	public static class Year {

		public static readonly Error NegativeYearWarning = new("Negative Year", ErrorSeverity.Warning,
			"A negative year does not have a clear meaning.");

		public const int FirstYearOfFirst = 1992;

		public static readonly Error YearPredatesFirstWarning = new("Year Predates FIRST", ErrorSeverity.Warning,
			"The year specified is before the year of the first FRC event.");

		public const int FutureYearAdvisoryThreshold = 1;
		public const int FutureYearWarningThreshold = 10;

		public static readonly Error FutureYearAdvisory = new("Future Year", ErrorSeverity.Advisory,
			$"The year specified is more than {FutureYearAdvisoryThreshold.ToWrittenConvention()} year in the future.");

		public static readonly Error FutureYearWarning = new("Future Year", ErrorSeverity.Warning,
			$"The year specified is more than {FutureYearWarningThreshold.ToWrittenConvention()} year in the future.");

	}

	public static class Name {

		public static class Length {

			public const int LowerErrorThreshold = 0;
			public const int LowerWarningThreshold = 2;
			public const int LowerAdvisoryThreshold = 4;

			public const int UpperErrorThreshold = 1000;
			public const int UpperWarningThreshold = 30;
			public const int UpperAdvisoryThreshold = 20;



			public static readonly Error TooShortError =
				new("Empty Name", ErrorSeverity.Error, "A Game cannot have a blank name.");

			public static readonly Error TooShortWarning =
				new("Short Name", ErrorSeverity.Warning, "The game name is alarmingly short.");

			public static readonly Error TooShortAdvisory =
				new("Short Name", ErrorSeverity.Advisory, "The game name is rather short.");



			public static readonly Error TooLongError
				= new("Long Name", ErrorSeverity.Advisory, "The game name is rather long.");

			public static readonly Error TooLongWarning
				= new("Long Name", ErrorSeverity.Warning, "The game name is alarmingly long");

			public static readonly Error TooLongAdvisory
				= new("Long Name", ErrorSeverity.Error, $"A game name cannot be more than {UpperErrorThreshold} characters.");

		}

	}

	public static class Version {

	}

}