using System;
using System.Collections.Generic;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Extensions;
using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Data;



public static class GeneralData {

	public static Error GetInvalidCharactersError(char[] invalidCharacters) {

		string invalidMessage = invalidCharacters.Length switch {
			0 => throw new ArgumentException($"It is expected that {nameof(invalidCharacters)} has at least one item."),
			1 => $"The character \"{invalidCharacters}\" is not valid.",
			_ => $"The characters \"{invalidCharacters}\" are not valid. "
		};

		return new("Invalid Characters", ErrorSeverity.Error, invalidMessage);
	}



	private static (Optional<T>, ReadOnlyList<Error>) ConvertToSignedInt<T>(string inputString, Func<string, T> parser,
		NumericString maxValue, NumericString minValue, IntConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		if (inputString.Count(x => x == '.') == 1 && inputString.All(x => char.IsDigit(x) || x == '.')) {
			return (Optional.NoValue, new(errorSet.MustBeInteger));
		}

		char[] invalidCharacters = inputString.Where(x => !char.IsDigit(x) && x != '-').ToArray();

		List<Error> errors = new();

		if (invalidCharacters.Any()) {
			errors.Add(errorSet.InvalidCharactersErrorGetter(invalidCharacters));
		}

		if (inputString.Count(x => x == '-') > 1 ||
		    inputString.Count(x => x == '-') == 1 && inputString.StartsWith('-') == false) {

			errors.Add(errorSet.NegativeSignMustBeAtStartError);
		}

		if (errors.Any()) {
			return (Optional.NoValue, errors.ToReadOnly());
		}

		if (inputString.NumericGreaterThan(maxValue)) {
			return (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString)));
		}

		if (inputString.NumericLessThan(minValue)) {
			return (Optional.NoValue, new(errorSet.ValueTooNegativeErrorGetter(inputString)));
		}

		return (parser.Invoke(inputString), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<int>, ReadOnlyList<Error>) ConvertToInt(string inputString, IntConversionErrorSet errorSet) {

		return ConvertToSignedInt(inputString, int.Parse, new(int.MaxValue), new(int.MinValue), errorSet);
	}



	private static (Optional<T>, ReadOnlyList<Error>) ConvertToUnsignedInt<T>(string inputString, Func<string, T> parser,
		NumericString maxValue, UintConversionErrorSet errorSet) {

		if (inputString.Length == 0) {
			return (Optional.NoValue, new(errorSet.RequiresValueError));
		}

		if (inputString.StartsWith('-') && inputString[1..].All(char.IsDigit)) {
			return (Optional.NoValue, new(errorSet.CannotBeNegativeError));
		}

		if (inputString.Count(x => x == '.') == 1 && inputString.All(x => char.IsDigit(x) || x == '.')) {
			return (Optional.NoValue, new(errorSet.MustBeInteger));
		}

		if (inputString.StartsWith('-') && 
		    inputString.Count(x => x == '.') == 1 && 
		    inputString[1..].All(x => char.IsDigit(x) || x == '.')) {

			return (Optional.NoValue, new(errorSet.MustBeInteger, errorSet.CannotBeNegativeError));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (Optional.NoValue, new(errorSet.InvalidCharactersErrorGetter(invalidCharacters)));
		}
		
		if (inputString.NumericGreaterThan(maxValue)) {
			return (Optional.NoValue, new(errorSet.ValueTooLargeErrorGetter(inputString)));
		}

		return (parser.Invoke(inputString), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<uint>, ReadOnlyList<Error>) ConvertToUint(string inputString, UintConversionErrorSet errorSet) {

		return ConvertToUnsignedInt(inputString, uint.Parse, new(uint.MaxValue), errorSet);
	}

	public static (Optional<byte>, ReadOnlyList<Error>) ConvertToByte(string inputString, UintConversionErrorSet errorSet) {

		return ConvertToUnsignedInt(inputString, byte.Parse, new(byte.MaxValue), errorSet);
	}

}



public class IntConversionErrorSet {

	//Todo: make required in .net7
	public Error RequiresValueError { get; /*required*/ init; }

	//Todo: make required in .net7
	public Func<char[], Error> InvalidCharactersErrorGetter { get; /*required*/ init; }

	//Todo: make required in .net7
	public Func<string, Error> ValueTooLargeErrorGetter { get; /*required*/ init; }

	//Todo: make required in .net7
	public Func<string, Error> ValueTooNegativeErrorGetter { get; /*required*/ init; }

	//Todo: make required in .net7
	public Error MustBeInteger { get; /*required*/ init; }

	//Todo: make required in .net7
	public Error NegativeSignMustBeAtStartError { get; /*required*/ init; }

	//Todo: replace with required properties in .net7
	public IntConversionErrorSet(Error requiresValueError, Func<char[], Error> invalidCharactersErrorGetter,
		Func<string, Error> valueTooLargeErrorGetter, Func<string, Error> valueTooNegativeErrorGetter,
		Error mustBeInteger, Error negativeSignMustBeAtStartError) {

		RequiresValueError = requiresValueError;
		InvalidCharactersErrorGetter = invalidCharactersErrorGetter;
		ValueTooLargeErrorGetter = valueTooLargeErrorGetter;
		ValueTooNegativeErrorGetter = valueTooNegativeErrorGetter;
		MustBeInteger = mustBeInteger;
		NegativeSignMustBeAtStartError = negativeSignMustBeAtStartError;
	}

}

public class UintConversionErrorSet {

	//Todo: make required in .net7
	public Error RequiresValueError { get; /*required*/ init; }

	//Todo: make required in .net7
	public Func<char[], Error> InvalidCharactersErrorGetter { get; /*required*/ init; }

	//Todo: make required in .net7
	public Func<string, Error> ValueTooLargeErrorGetter { get; /*required*/ init; }

	//Todo: make required in .net7
	public Error CannotBeNegativeError { get; /*required*/ init; }

	//Todo: make required in .net7
	public Error MustBeInteger { get; /*required*/ init; }

	//Todo: replace with required properties in .net7
	public UintConversionErrorSet(Error requiresValueError, Func<char[], Error> invalidCharactersErrorGetter,
		Func<string, Error> valueTooLargeErrorGetter, Error cannotBeNegativeError, Error mustBeInteger) {

		RequiresValueError = requiresValueError;
		InvalidCharactersErrorGetter = invalidCharactersErrorGetter;
		ValueTooLargeErrorGetter = valueTooLargeErrorGetter;
		CannotBeNegativeError = cannotBeNegativeError;
		MustBeInteger = mustBeInteger;
	}

}



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

		public const string ShouldEndWith = " Alliance";

		public static readonly Error DoesNotEndWithCorrectSequenceError
			= new("Does Not End With \"Alliance\"", ErrorSeverity.Error,
				"Typically alliance names should follow the format \"{Colour} Alliance\".");

	}

	public static class Color {

		public static class Component {

			public const string EntryInstruction = "Please enter an integer between 0 and 255.";
			public const string EnteredText = "An integer between 0 and 255.";

			private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
				"A color value must be specified.");

			private static Error GetInvalidCharactersError(char[] invalidCharacters) {
				return GeneralData.GetInvalidCharactersError(invalidCharacters);
			}

			private static Error ValueTooLargeErrorGetter(string givenValue) {

				return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to" +
				                                                   $" a color value. The maximum possible value is {byte.MaxValue}.");
			}

			private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
				"A color value cannot be a negative number.");

			private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
				"A color value must be a whole number.");

			public static readonly UintConversionErrorSet ConversionErrorSet = new(RequiresValueError, GetInvalidCharactersError,
				ValueTooLargeErrorGetter, CannotBeNegativeError, MustBeIntegerError);

		}

		private const int SimilarityErrorThreshold = 0;
		private const int SimilarityWarningThreshold = 10;
		private const int SimilarityAdvisoryThreshold = 20;

		public static Optional<Error> GetColorSimilarityError(int colorDifference, string otherAllianceName) {

			return colorDifference switch {
				<= SimilarityErrorThreshold => new Error("Identical Color", ErrorSeverity.Error,
					$"The alliance color is identical to that of the {otherAllianceName}"),

				<= SimilarityWarningThreshold => new Error("Similar Color", ErrorSeverity.Warning,
					$"The alliance color is very similar to that of the {otherAllianceName}"),

				<= SimilarityAdvisoryThreshold => new Error("Similar Color", ErrorSeverity.Advisory,
					$"The alliance color is similar to that of the {otherAllianceName}"),

				_ => Optional.NoValue
			};
		}

	}

}



public static class GameData {

	public static class Year {

		public const string EntryInstruction = "Please enter an integer.";
		public const string EnteredText = "An integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error, "A year must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return GeneralData.GetInvalidCharactersError(invalidCharacters);
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

		private static readonly Error MinusMustBeAtStartError = new("Improper Minus Sign", ErrorSeverity.Error, 
			"A minus sign is only valid as the first character.");

		public static readonly IntConversionErrorSet ConversionErrorSet = new(RequiresValueError, GetInvalidCharactersError,
			ValueTooLargeErrorGetter, ValueTooNegativeErrorGetter, MustBeIntegerError, MinusMustBeAtStartError);



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
			return GeneralData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a number of " +
			                                                   $"robots per alliance. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"The number of robots per alliance cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"The number of robots per alliance must be a whole number.");

		public static readonly UintConversionErrorSet ConversionErrorSet = new(RequiresValueError, GetInvalidCharactersError,
			ValueTooLargeErrorGetter, CannotBeNegativeError, MustBeIntegerError);

	}

	public static class AlliancesPerMatch {

		public const string EntryInstruction = "Please enter a positive integer.";
		public const string EnteredText = "A positive integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"The number of alliances per match must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return GeneralData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a number of " +
			                                                   $"alliances per match. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"The number of alliances per match cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"The number of alliances per match must be a whole number.");

		public static readonly UintConversionErrorSet ConversionErrorSet = new(RequiresValueError, GetInvalidCharactersError,
			ValueTooLargeErrorGetter, CannotBeNegativeError, MustBeIntegerError);

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

}

public static class VersionData {

	public static class ComponentNumber {
			
		public const string EntryInstruction = "Please enter a positive integer.";
		public const string EnteredText = "A positive integer.";

		private static readonly Error RequiresValueError = new("Requires Value", ErrorSeverity.Error,
			"A version component number must be specified.");

		private static Error GetInvalidCharactersError(char[] invalidCharacters) {
			return GeneralData.GetInvalidCharactersError(invalidCharacters);
		}

		private static Error ValueTooLargeErrorGetter(string givenValue) {

			return new("Value Too Large", ErrorSeverity.Error, $"The given value, {givenValue}, is too large to be converted to a version" +
			                                                   $" component number. The maximum possible value is {uint.MaxValue}.");
		}

		private static readonly Error CannotBeNegativeError = new("Cannot Be Negative", ErrorSeverity.Error,
			"A version component number cannot be a negative number.");

		private static readonly Error MustBeIntegerError = new("Must Be Integer", ErrorSeverity.Error, 
			"A version component number must be a whole number.");

		public static readonly UintConversionErrorSet ConversionErrorSet = new(RequiresValueError, GetInvalidCharactersError,
			ValueTooLargeErrorGetter, CannotBeNegativeError, MustBeIntegerError);

	}

}