using System;
using CCSSDomain.Data;
using WPFUtilities;
using WPFUtilities.Validation;
using WPFUtilities.Validation.Delegates;
using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Game;



public static class GameVersionValidator {

	private static (Optional<Version>, Optional<Error>) Converter
		((uint major, uint minor, uint path, string description) input) {

		if (input.description is null) {
			throw new NullInputObjectInConverter();
		}

		return (new Version(input.major, input.minor, input.path, input.description), Optional.NoValue);
	}

	private static (Optional<(uint, uint, uint, string)>, Optional<Error>) Inverter(Version version) {

		if (version is null) {
			throw new NullInputObjectInInverter();
		}

		return ((version.MajorNumber, version.MinorNumber, version.PatchNumber, version.Description), Optional.NoValue);
	}

	public static readonly ConversionPair<Version, (uint, uint, uint, string), ErrorSeverity> ConversionPair
		= new(Converter, Inverter);



	private static (Optional<uint>, ReadOnlyList<Error>) ComponentNumberConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return GeneralData.ConvertToUint(inputString, VersionData.ComponentNumber.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) ComponentNumberInverter(uint versionNumberComponent) {

		return (versionNumberComponent.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> ComponentNumberConversionPair 
		= new(ComponentNumberConverter, ComponentNumberInverter);



	private static (Optional<string>, Optional<Error>) DescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) DescriptionInverter(string versionDescription) {

		if (versionDescription is null) {
			throw new NullInputObjectInInverter();
		}

		return (versionDescription, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> DescriptionConversionPair 
		= new(DescriptionConverter, DescriptionInverter);

}



public static class GameTextValidator {

	private static (Optional<string>, Optional<Error>) NameConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) NameInverter(string name) {

		if (name is null) {
			throw new NullInputObjectInInverter();
		}

		return (name, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> VersionNameConversionPair 
		= new(NameConverter, NameInverter);



	public static Optional<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= GameData.Name.Length.LowerErrorThreshold => GameData.Name.Length.TooShortError,
			<= GameData.Name.Length.LowerWarningThreshold => GameData.Name.Length.TooShortWarning,
			<= GameData.Name.Length.LowerAdvisoryThreshold => GameData.Name.Length.TooShortAdvisory,
			>= GameData.Name.Length.UpperErrorThreshold => GameData.Name.Length.TooLongError,
			>= GameData.Name.Length.UpperWarningThreshold => GameData.Name.Length.TooLongWarning,
			>= GameData.Name.Length.UpperAdvisoryThreshold => GameData.Name.Length.TooLongAdvisory,
			_ => Optional.NoValue
		};

	}



	private static (Optional<string>, Optional<Error>) DescriptionConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) DescriptionInverter(string description) {

		if (description is null) {
			throw new NullInputObjectInInverter();
		}

		return (description, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> DescriptionConversionPair 
		= new(DescriptionConverter, DescriptionInverter);

}



public static class GameNumbersValidator {

	private static (Optional<int>, ReadOnlyList<Error>) YearConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return GeneralData.ConvertToInt(inputString, GameData.Year.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) YearInverter(int year) {

		return (year.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<int, string, ErrorSeverity> YearConversionPair = new(YearConverter, YearInverter);



	public static Optional<Error> YearValidator_YearNotNegative(int year) {

		if (year < 0) {
			return GameData.Year.NegativeYearWarning;
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotPredateFirst(int year) {

		if (year < GameData.Year.FirstYearOfFirst) {
			return GameData.Year.YearPredatesFirstWarning;
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotFarFuture(int year) {

		if (year < DateTime.Now.Year + GameData.Year.FutureYearAdvisoryThreshold) {
			return GameData.Year.FutureYearAdvisory;
		}

		if (year < DateTime.Now.Year + GameData.Year.FutureYearWarningThreshold) {
			return GameData.Year.FutureYearWarning;
		}

		return Optional.NoValue;
	}



	private static (Optional<uint>, ReadOnlyList<Error>) RobotsPerAllianceConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return GeneralData.ConvertToUint(inputString, GameData.RobotsPerAlliance.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) RobotsPerAllianceInverter(uint robotsPerAlliance) {

		return (robotsPerAlliance.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> RobotsPerAllianceConversionPair
		= new(RobotsPerAllianceConverter, RobotsPerAllianceInverter);



	private static (Optional<uint>, ReadOnlyList<Error>) AlliancesPerMatchConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		return GeneralData.ConvertToUint(inputString, GameData.AlliancesPerMatch.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) AlliancesPerMatchInverter(uint alliancesPerMatch) {

		return (alliancesPerMatch.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> AlliancesPerMatchConversionPair
		= new(AlliancesPerMatchConverter, AlliancesPerMatchInverter);

}