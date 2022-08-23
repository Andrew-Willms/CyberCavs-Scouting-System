using System;
using CCSSDomain;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Delegates;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using Version = CCSSDomain.Version;

namespace GameMakerWpf.Validation.Validators;



public static class GameVersionValidator {

	private static (Optional<Version>, Optional<Error>) Converter
		((uint major, uint minor, uint path, string description) input) {

		NullInputObjectInConverterException.ThrowIfNull(input.description);

		return (new Version(input.major, input.minor, input.path, input.description), Optional.NoValue);
	}

	private static (Optional<(uint, uint, uint, string)>, Optional<Error>) Inverter(Version version) {

		NullInputObjectInInverterException.ThrowIfNull(version);

		return ((version.MajorNumber, version.MinorNumber, version.PatchNumber, version.Description), Optional.NoValue);
	}

	public static readonly ConversionPair<Version, (uint, uint, uint, string), ErrorSeverity> ConversionPair
		= new(Converter, Inverter);



	private static (Optional<uint>, ReadOnlyList<Error>) ComponentNumberConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, VersionValidationData.ComponentNumber.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) ComponentNumberInverter(uint versionNumberComponent) {

		return (versionNumberComponent.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> ComponentNumberConversionPair
		= new(ComponentNumberConverter, ComponentNumberInverter);



	private static (Optional<string>, Optional<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) DescriptionInverter(string versionDescription) {

		NullInputObjectInInverterException.ThrowIfNull(versionDescription);

		return (versionDescription, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> DescriptionConversionPair
		= new(DescriptionConverter, DescriptionInverter);

}



public static class GameTextValidator {

	private static (Optional<string>, Optional<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> NameConversionPair
		= new(NameConverter, NameInverter);



	public static Optional<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= GameValidationData.Name.Length.LowerErrorThreshold => GameValidationData.Name.Length.TooShortError,
			<= GameValidationData.Name.Length.LowerWarningThreshold => GameValidationData.Name.Length.TooShortWarning,
			<= GameValidationData.Name.Length.LowerAdvisoryThreshold => GameValidationData.Name.Length.TooShortAdvisory,
			>= GameValidationData.Name.Length.UpperErrorThreshold => GameValidationData.Name.Length.TooLongError,
			>= GameValidationData.Name.Length.UpperWarningThreshold => GameValidationData.Name.Length.TooLongWarning,
			>= GameValidationData.Name.Length.UpperAdvisoryThreshold => GameValidationData.Name.Length.TooLongAdvisory,
			_ => Optional.NoValue
		};

	}



	private static (Optional<string>, Optional<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) DescriptionInverter(string description) {

		NullInputObjectInInverterException.ThrowIfNull(description);

		return (description, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> DescriptionConversionPair
		= new(DescriptionConverter, DescriptionInverter);

}



public static class GameNumbersValidator {

	private static (Optional<int>, ReadOnlyList<Error>) YearConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, GameValidationData.Year.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) YearInverter(int year) {

		return (year.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<int, string, ErrorSeverity> YearConversionPair = new(YearConverter, YearInverter);



	public static Optional<Error> YearValidator_YearNotNegative(int year) {

		if (year < 0) {
			return GameValidationData.Year.NegativeYearWarning;
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotPredateFirst(int year) {

		if (year < GameValidationData.Year.FirstYearOfFirst) {
			return GameValidationData.Year.YearPredatesFirstWarning;
		}

		return Optional.NoValue;
	}

	public static Optional<Error> YearValidator_YearNotFarFuture(int year) {

		if (year > DateTime.Now.Year + GameValidationData.Year.FutureYearWarningThreshold) {
			return GameValidationData.Year.FutureYearWarning;
		}

		if (year > DateTime.Now.Year + GameValidationData.Year.FutureYearAdvisoryThreshold) {
			return GameValidationData.Year.FutureYearAdvisory;
		}

		return Optional.NoValue;
	}



	private static (Optional<uint>, ReadOnlyList<Error>) RobotsPerAllianceConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.RobotsPerAlliance.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) RobotsPerAllianceInverter(uint robotsPerAlliance) {

		return (robotsPerAlliance.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> RobotsPerAllianceConversionPair
		= new(RobotsPerAllianceConverter, RobotsPerAllianceInverter);



	private static (Optional<uint>, ReadOnlyList<Error>) AlliancesPerMatchConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.AlliancesPerMatch.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) AlliancesPerMatchInverter(uint alliancesPerMatch) {

		return (alliancesPerMatch.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<uint, string, ErrorSeverity> AlliancesPerMatchConversionPair
		= new(AlliancesPerMatchConverter, AlliancesPerMatchInverter);

}