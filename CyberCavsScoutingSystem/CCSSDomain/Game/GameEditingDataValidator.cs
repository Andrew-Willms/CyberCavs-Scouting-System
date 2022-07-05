using System;
using System.Linq;

using WPFUtilities;
using WPFUtilities.Extensions;
using WPFUtilities.Validation.Errors;

namespace CCSSDomain.Game;



public static class GameEditingDataValidator {

	public static (VersionNumber?, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionConverter
		(uint majorVersionNumber, uint minorVersionNumber, uint patchVersionNumber, string versionDescription) {

		return (new(majorVersionNumber, minorVersionNumber, patchVersionNumber) { VersionDescription = versionDescription },
			ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (uint, uint, uint, string, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionInverter
		(VersionNumber versionNumber) {

		return (versionNumber.MajorNumber, versionNumber.MinorNumber, versionNumber.PatchNumber, versionNumber.VersionDescription,
			ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (uint, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionNumberComponentConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new(new ValidationError<ErrorSeverity>("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value.")));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new(new ValidationError<ErrorSeverity>("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field.")));
		}

		if (inputString.NumericCompare(uint.MaxValue.ToString()) > 1) {
			return (0, new(new ValidationError<ErrorSeverity>("Number Too Large", ErrorSeverity.Error, "Too big to convert to int.")));
		}

		return (uint.Parse(inputString), new());
	}

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionNumberComponentInverter
		(uint versionNumberComponent) {

		return (versionNumberComponent.ToString(), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}



	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionDescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, new());
	}

	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) VersionDescriptionInverter(string versionDescription) {

		if (versionDescription is null) {
			throw new ArgumentNullException(nameof(versionDescription), "You shouldn't be able to send a null string to this validator.");
		}

		return (versionDescription, new());
	}



	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) NameConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) NameInverter(string name) {

		return (name, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Length(string name) {

		//Todo extract this magic numbers.
		return name.Length switch {
			0 => new(new ValidationError<ErrorSeverity>("Empty Name", ErrorSeverity.Warning, "This name is empty.")),
			< 5 => new(new ValidationError<ErrorSeverity>("Short Name", ErrorSeverity.Warning, "This name is alarmingly short.")),
			< 10 => new(new ValidationError<ErrorSeverity>("Short Name", ErrorSeverity.Warning, "This name is rather short.")),
			> 40 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Warning, "This alliance name is alarmingly long.")),
			> 30 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Advisory, "This alliance name is rather long.")),
			_ => ReadOnlyList<ValidationError<ErrorSeverity>>.Empty
		};
	}



	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) DescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, new());
	}

	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) DescriptionInverter(string description) {

		if (description is null) {
			throw new ArgumentNullException(nameof(description), "You shouldn't be able to send a null string to this validator.");
		}

		return (description, new());
	}



	public static (int, ReadOnlyList<ValidationError<ErrorSeverity>>) YearConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new(new ValidationError<ErrorSeverity>("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value.")));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new(new ValidationError<ErrorSeverity>("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field.")));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new(new ValidationError<ErrorSeverity>("Number Too Large", ErrorSeverity.Error, "Too big to convert to int.")));
		}

		return (int.Parse(inputString), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) YearInverter(int year) {

		return (year.ToString(), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> YearValidator_YearNotNegative(int year) {

		if (year < 0) {
			return new(new ValidationError<ErrorSeverity>("Negative Year", ErrorSeverity.Warning,
				"Why are you specifying a negative year???"));
		}

		return ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> YearValidator_YearNotPredateFirst(int year) {

		if (year < 1992) {
			return new(new ValidationError<ErrorSeverity>("Old Year", ErrorSeverity.Advisory,
				"The year specified is before the year of the first FRC event."));
		}

		return ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> YearValidator_YearNotFarFuture(int year) {

		if (year < DateTime.Now.Year + 1) {
			return new(new ValidationError<ErrorSeverity>("Distant Year", ErrorSeverity.Advisory,
				"The year specified is more than a year in the future."));
		}

		return ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
	}



	public static (int, ReadOnlyList<ValidationError<ErrorSeverity>>) RobotsPerAllianceConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new(new ValidationError<ErrorSeverity>("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value.")));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new(new ValidationError<ErrorSeverity>("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field.")));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new(new ValidationError<ErrorSeverity>("Number Too Large", ErrorSeverity.Error, "Too big to convert to int.")));
		}

		return (int.Parse(inputString), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) RobotsPerAllianceInverter(int robotsPerAlliance) {

		return (robotsPerAlliance.ToString(), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}



	public static (int, ReadOnlyList<ValidationError<ErrorSeverity>>) AlliancesPerMatchConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new(new ValidationError<ErrorSeverity>("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value.")));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new(new ValidationError<ErrorSeverity>("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field.")));
		}

		if (inputString.NumericCompare(int.MaxValue.ToString()) > 1) {
			return (0, new(new ValidationError<ErrorSeverity>("Number Too Large", ErrorSeverity.Error, "Too big to convert to int.")));
		}

		return (int.Parse(inputString), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) AlliancesPerMatchInverter(int alliancesPerMatch) {

		return (alliancesPerMatch.ToString(), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

}
