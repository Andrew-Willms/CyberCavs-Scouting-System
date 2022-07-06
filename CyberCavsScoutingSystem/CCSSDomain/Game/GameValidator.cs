using System;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Extensions;
using WPFUtilities.Validation.Delegates;

using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Game;



public static class GameValidator {

	private static (Optional<Version>, Optional<Error>) VersionConverter
		((uint major, uint minor, uint path, string description) input) {

		return ((new Version(input.major, input.minor, input.path, input.description)), Optional.NoValue);
	}

	private static (Optional<(uint, uint, uint, string)>, Optional<Error>) VersionInverter(Version version) {

		return (((version.MajorNumber, version.MinorNumber, version.PatchNumber, version.Description)), Optional.NoValue);
	}

	public static readonly ConversionPair<Version, (uint, uint, uint, string), ErrorSeverity>
		VersionConversionPair = new(VersionConverter, VersionInverter);



	private static (Optional<uint>, Optional<Error>) VersionComponentNumberConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new Error("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new Error("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(uint.MaxValue.ToString()) > 1) {
			return (0, new Error("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (uint.Parse(inputString), Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) VersionComponentNumberInverter
		(uint versionNumberComponent) {

		return (versionNumberComponent.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> VersionComponentNumberConversionPair 
		= new(VersionComponentNumberConverter, VersionComponentNumberInverter);



	private static (Optional<string>, Optional<Error>) VersionDescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) VersionDescriptionInverter(string versionDescription) {

		if (versionDescription is null) {
			throw new ArgumentNullException(nameof(versionDescription), "You shouldn't be able to send a null string to this validator.");
		}

		return (versionDescription, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> VersionDescriptionConversionPair 
		= new(VersionDescriptionConverter, VersionDescriptionInverter);



	private static (Optional<string>, Optional<Error>) NameConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) NameInverter(string name) {

		return (name, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> VersionNameConversionPair 
		= new(NameConverter, NameInverter);



	public static Optional<Error> NameValidator_Length(string name) {

		//Todo extract this magic numbers.
		return name.Length switch {
			0 => new Error("Empty Name", ErrorSeverity.Warning, "This name is empty."),
			< 5 => new Error("Short Name", ErrorSeverity.Warning, "This name is alarmingly short."),
			< 10 => new Error("Short Name", ErrorSeverity.Warning, "This name is rather short."),
			> 40 => new Error("Long Name", ErrorSeverity.Warning, "This alliance name is alarmingly long."),
			> 30 => new Error("Long Name", ErrorSeverity.Advisory, "This alliance name is rather long."),
			_ => Optional.NoValue
		};
	}



	public static (string?, Optional<Error>) DescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, Optional.NoValue);
	}

	public static (string?, Optional<Error>) DescriptionInverter(string description) {

		if (description is null) {
			throw new ArgumentNullException(nameof(description), "You shouldn't be able to send a null string to this validator.");
		}

		return (description, Optional.NoValue);
	}



	private static (Optional<int>, Optional<Error>) YearConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new Error("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new Error("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new Error("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (int.Parse(inputString), Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) YearInverter(int year) {

		return (year.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<int, string, ErrorSeverity> YearConversionPair = new(YearConverter, YearInverter);



	public static Optional<Error> YearValidator_YearNotNegative(int year) {

		if (year < 0) {
			return new Error("Negative Year", ErrorSeverity.Warning,
				"Why are you specifying a negative year???");
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotPredateFirst(int year) {

		if (year < 1992) {
			return new Error("Old Year", ErrorSeverity.Advisory,
				"The year specified is before the year of the first FRC event.");
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotFarFuture(int year) {

		if (year < DateTime.Now.Year + 1) {
			return new Error("Distant Year", ErrorSeverity.Advisory,
				"The year specified is more than a year in the future.");
		}

		return Optional.NoValue;
	}



	private static (Optional<int>, Optional<Error>) RobotsPerAllianceConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new Error("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new Error("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new Error("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (int.Parse(inputString), Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) RobotsPerAllianceInverter(int robotsPerAlliance) {

		return (robotsPerAlliance.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<int, string, ErrorSeverity> RobotsPerAllianceConversionPair
		= new(RobotsPerAllianceConverter, RobotsPerAllianceInverter);



	private static (Optional<int>, Optional<Error>) AlliancesPerMatchConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new Error("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new Error("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new Error("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (int.Parse(inputString), Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) AlliancesPerMatchInverter(int alliancesPerMatch) {

		return (alliancesPerMatch.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<int, string, ErrorSeverity> AlliancesPerMatchConversionPair
		= new(AlliancesPerMatchConverter, AlliancesPerMatchInverter);

}