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

	public static (Optional<Version>, ReadOnlyList<Error>) Converter
		((uint major, uint minor, uint path, string description) input) {

		NullInputObjectInConverterException.ThrowIfNull(input.description);

		return (new Version(input.major, input.minor, input.path, input.description), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<(uint, uint, uint, string)>, ReadOnlyList<Error>) Inverter(Version version) {

		NullInputObjectInInverterException.ThrowIfNull(version);

		return ((version.MajorNumber, version.MinorNumber, version.PatchNumber, version.Description), ReadOnlyList<Error>.Empty);
	}



	public static (Optional<uint>, ReadOnlyList<Error>) ComponentNumberConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, VersionValidationData.ComponentNumber.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) ComponentNumberInverter(uint versionNumberComponent) {

		return (versionNumberComponent.ToString(), ReadOnlyList<Error>.Empty);
	}



	public static (Optional<string>, ReadOnlyList<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DescriptionInverter(string versionDescription) {

		NullInputObjectInInverterException.ThrowIfNull(versionDescription);

		return (versionDescription, ReadOnlyList<Error>.Empty);
	}

}



public static class GameTextValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name, ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= GameValidationData.Name.Length.LowerErrorThreshold => new(GameValidationData.Name.Length.TooShortError),
			<= GameValidationData.Name.Length.LowerWarningThreshold => new(GameValidationData.Name.Length.TooShortWarning),
			<= GameValidationData.Name.Length.LowerAdvisoryThreshold => new(GameValidationData.Name.Length.TooShortAdvisory),
			>= GameValidationData.Name.Length.UpperErrorThreshold => new(GameValidationData.Name.Length.TooLongError),
			>= GameValidationData.Name.Length.UpperWarningThreshold => new(GameValidationData.Name.Length.TooLongWarning),
			>= GameValidationData.Name.Length.UpperAdvisoryThreshold => new(GameValidationData.Name.Length.TooLongAdvisory),
			_ => ReadOnlyList<Error>.Empty
		};

	}



	public static (Optional<string>, ReadOnlyList<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DescriptionInverter(string description) {

		NullInputObjectInInverterException.ThrowIfNull(description);

		return (description, ReadOnlyList<Error>.Empty);
	}

}



public static class GameNumbersValidator {

	public static (Optional<int>, ReadOnlyList<Error>) YearConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, GameValidationData.Year.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) YearInverter(int year) {

		return (year.ToString(), ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> YearValidator_YearNotNegative(int year) {

		return year < 0
			? new(GameValidationData.Year.NegativeYearWarning)
			: ReadOnlyList<Error>.Empty;
	}

	public static ReadOnlyList<Error> YearValidator_YearNotPredateFirst(int year) {

		return year < GameValidationData.Year.FirstYearOfFirst 
			? new(GameValidationData.Year.YearPredatesFirstWarning) 
			: ReadOnlyList<Error>.Empty;
	}

	public static ReadOnlyList<Error> YearValidator_YearNotFarFuture(int year) {

		if (year > DateTime.Now.Year + GameValidationData.Year.FutureYearWarningThreshold) {
			return new(GameValidationData.Year.FutureYearWarning);
		}

		return year > DateTime.Now.Year + GameValidationData.Year.FutureYearAdvisoryThreshold 
			? new(GameValidationData.Year.FutureYearAdvisory) 
			: ReadOnlyList<Error>.Empty;
	}



	public static (Optional<uint>, ReadOnlyList<Error>) RobotsPerAllianceConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.RobotsPerAlliance.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) RobotsPerAllianceInverter(uint robotsPerAlliance) {

		return (robotsPerAlliance.ToString(), ReadOnlyList<Error>.Empty);
	}



	public static (Optional<uint>, ReadOnlyList<Error>) AlliancesPerMatchConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.AlliancesPerMatch.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) AlliancesPerMatchInverter(uint alliancesPerMatch) {

		return (alliancesPerMatch.ToString(), ReadOnlyList<Error>.Empty);
	}

}