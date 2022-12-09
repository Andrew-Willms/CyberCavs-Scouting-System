using System;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Validation;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using Version = CCSSDomain.Models.Version;

namespace GameMakerWpf.Validation.Validators;



public static class GameVersionValidator {

	public static (Optional<Version>, ReadOnlyList<Error>) Converter
		((uint major, uint minor, uint path, string description) input) {

		NullInputObjectInConverterException.ThrowIfNull(input.description);

		return (new Version(input.major, input.minor, input.path, input.description).Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<(uint, uint, uint, string)>, ReadOnlyList<Error>) Inverter(Version version) {

		NullInputObjectInInverterException.ThrowIfNull(version);

		return ((version.MajorNumber, version.MinorNumber, version.PatchNumber, version.Description).Optionalize(), ReadOnlyList.Empty);
	}



	public static (Optional<uint>, ReadOnlyList<Error>) ComponentNumberConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, VersionValidationData.ComponentNumber.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) ComponentNumberInverter(uint versionNumberComponent) {

		return (versionNumberComponent.ToString().Optionalize(), ReadOnlyList.Empty);
	}



	public static (Optional<string>, ReadOnlyList<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DescriptionInverter(string versionDescription) {

		NullInputObjectInInverterException.ThrowIfNull(versionDescription);

		return (versionDescription.Optionalize(), ReadOnlyList.Empty);
	}

}



public static class GameTextValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= GameValidationData.Name.Length.LowerErrorThreshold => GameValidationData.Name.Length.TooShortError.ReadOnlyListify(),
			<= GameValidationData.Name.Length.LowerWarningThreshold => GameValidationData.Name.Length.TooShortWarning.ReadOnlyListify(),
			<= GameValidationData.Name.Length.LowerAdvisoryThreshold => GameValidationData.Name.Length.TooShortAdvisory.ReadOnlyListify(),
			>= GameValidationData.Name.Length.UpperErrorThreshold => GameValidationData.Name.Length.TooLongError.ReadOnlyListify(),
			>= GameValidationData.Name.Length.UpperWarningThreshold => GameValidationData.Name.Length.TooLongWarning.ReadOnlyListify(),
			>= GameValidationData.Name.Length.UpperAdvisoryThreshold => GameValidationData.Name.Length.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};

	}



	public static (Optional<string>, ReadOnlyList<Error>) DescriptionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DescriptionInverter(string description) {

		NullInputObjectInInverterException.ThrowIfNull(description);

		return (description.Optionalize(), ReadOnlyList.Empty);
	}

}



public static class GameNumbersValidator {

	public static (Optional<int>, ReadOnlyList<Error>) YearConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, GameValidationData.Year.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) YearInverter(int year) {

		return (year.ToString().Optionalize(), ReadOnlyList.Empty);
	}



	public static ReadOnlyList<Error> YearValidator_YearNotNegative(int year) {

		return year < 0
			? GameValidationData.Year.NegativeYearWarning.ReadOnlyListify()
			: ReadOnlyList.Empty;
	}

	public static ReadOnlyList<Error> YearValidator_YearNotPredateFirst(int year) {

		return year < GameValidationData.Year.FirstYearOfFirst 
			? GameValidationData.Year.YearPredatesFirstWarning.ReadOnlyListify()
			: ReadOnlyList.Empty;
	}

	public static ReadOnlyList<Error> YearValidator_YearNotFarFuture(int year) {

		if (year > DateTime.Now.Year + GameValidationData.Year.FutureYearWarningThreshold) {
			return GameValidationData.Year.FutureYearWarning.ReadOnlyListify();
		}

		return year > DateTime.Now.Year + GameValidationData.Year.FutureYearAdvisoryThreshold 
			? GameValidationData.Year.FutureYearAdvisory.ReadOnlyListify()
			: ReadOnlyList.Empty;
	}



	public static (Optional<uint>, ReadOnlyList<Error>) RobotsPerAllianceConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.RobotsPerAlliance.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) RobotsPerAllianceInverter(uint robotsPerAlliance) {

		return (robotsPerAlliance.ToString().Optionalize(), ReadOnlyList.Empty);
	}



	public static (Optional<uint>, ReadOnlyList<Error>) AlliancesPerMatchConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToUint(inputString, GameValidationData.AlliancesPerMatch.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) AlliancesPerMatchInverter(uint alliancesPerMatch) {

		return (alliancesPerMatch.ToString().Optionalize(), ReadOnlyList.Empty);
	}

}