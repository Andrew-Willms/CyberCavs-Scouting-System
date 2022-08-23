using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CCSSDomain;
using GameMakerWpf.EditingData;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Extensions;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Delegates;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators;



public static class AllianceValidator {

	private static (Optional<string>, Optional<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name, Optional.NoValue);
	}

	public static readonly ConversionPair<string, string, ErrorSeverity> NameConversionPair = new(NameConverter, NameInverter);



	public static Optional<Error> NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(AllianceValidationData.Name.ShouldEndWith)
			? Optional.NoValue
			: AllianceValidationData.Name.DoesNotEndWithCorrectSequenceError;
	}

	public static Optional<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceValidationData.Name.Length.LowerErrorThreshold => AllianceValidationData.Name.Length.TooShortError,
			<= AllianceValidationData.Name.Length.LowerWarningThreshold => AllianceValidationData.Name.Length.TooShortWarning,
			<= AllianceValidationData.Name.Length.LowerAdvisoryThreshold => AllianceValidationData.Name.Length.TooShortAdvisory,
			>= AllianceValidationData.Name.Length.UpperErrorThreshold => AllianceValidationData.Name.Length.TooLongError,
			>= AllianceValidationData.Name.Length.UpperWarningThreshold => AllianceValidationData.Name.Length.TooLongWarning,
			>= AllianceValidationData.Name.Length.UpperAdvisoryThreshold => AllianceValidationData.Name.Length.TooLongAdvisory,
			_ => Optional.NoValue
		};
	}

	public static Optional<Error> NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return otherAlliances.Any(otherAlliance => otherAlliance.Name.OutputObject == name)
			? AllianceValidationData.Name.GetDuplicateNameError(name)
			: Optional.NoValue;
	}



	private static (Optional<byte>, ReadOnlyList<Error>) ColorComponentConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToByte(inputString, AllianceValidationData.Color.Component.ConversionErrorSet);
	}

	private static (Optional<string>, Optional<Error>) ColorComponentInverter(byte colourComponentValue) {

		return (colourComponentValue.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<byte, string, ErrorSeverity> ColorComponentConversionPair
		= new(ColorComponentConverter, ColorComponentInverter);



	private static (Optional<Color>, Optional<Error>) ColorConverter((byte redValue, byte greenValue, byte blueValue) input) {

		return (Color.FromRgb(input.redValue, input.greenValue, input.blueValue), Optional.NoValue);
	}

	private static (Optional<(byte redValue, byte greenValue, byte blueValue)> invertedValues, Optional<Error>) ColorInverter
		(Color color) {

		return ((color.R, color.G, color.B), Optional.NoValue);
	}

	public static readonly ConversionPair<Color, (byte, byte, byte), ErrorSeverity> ColorConversionPair
		= new(ColorConverter, ColorInverter);



	public static ReadOnlyList<Error> ColorCovalidator_Uniqueness(Color color,
		IEnumerable<AllianceEditingData> otherAlliances) {

		List<Error> validationErrors = new();

		foreach (AllianceEditingData otherAlliance in otherAlliances) {

			int colorDifference = color.Difference(otherAlliance.AllianceColor.OutputObject);

			Optional<Error> validationError = AllianceValidationData.Color.GetColorSimilarityError(colorDifference, otherAlliance.Name.InputObject);

			validationErrors.AddIfHasValue(validationError);
		}

		return validationErrors.ToReadOnly();
	}

}