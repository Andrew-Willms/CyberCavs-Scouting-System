using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Extensions;
using WPFUtilities.Validation.Delegates;
using CCSSDomain.Data;
using WPFUtilities.Validation;
using Error = WPFUtilities.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace CCSSDomain.Alliance;



public static class AllianceValidator {

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
	
	public static readonly ConversionPair<string, string, ErrorSeverity> NameConversionPair = new(NameConverter, NameInverter);



	public static Optional<Error> NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(" Alliance")
			? Optional.NoValue
			: AllianceData.Name.DoesNotEndWithAllianceError;
	}

	public static Optional<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceData.Name.Length.LowerErrorThreshold => AllianceData.Name.Length.TooShortError,
			<= AllianceData.Name.Length.LowerWarningThreshold => AllianceData.Name.Length.TooShortWarning,
			<= AllianceData.Name.Length.LowerAdvisoryThreshold => AllianceData.Name.Length.TooShortAdvisory,
			>= AllianceData.Name.Length.UpperErrorThreshold => AllianceData.Name.Length.TooLongError,
			>= AllianceData.Name.Length.UpperWarningThreshold => AllianceData.Name.Length.TooLongWarning,
			>= AllianceData.Name.Length.UpperAdvisoryThreshold => AllianceData.Name.Length.TooLongAdvisory,
			_ => Optional.NoValue
		};
	}

	public static Optional<Error> NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return otherAlliances.Any(otherAlliance => otherAlliance.Name.OutputObject == name)
			? AllianceData.Name.GetDuplicateNameError(name)
			: Optional.NoValue;
	}



	private static (Optional<byte>, Optional<Error>) ColorComponentConverter(string inputString) {

		if (inputString is null) {
			throw new NullInputObjectInConverter();
		}

		if (inputString.Length == 0) {
			return (0, new Error("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new Error("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(byte.MaxValue.ToString()) > 1) {
			return (0, new Error("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (byte.Parse(inputString), Optional.NoValue);
	}

	private static (Optional<string>, Optional<Error>) ColorComponentInverter(byte colourComponentValue) {

		return (colourComponentValue.ToString(), Optional.NoValue);
	}

	public static readonly ConversionPair<byte, string, ErrorSeverity> ColorComponentConversionPair
		= new(ColorComponentConverter, ColorComponentInverter);



	private static (Optional<Color>, Optional<Error>) ColorConverter
		((byte redValue, byte greenValue, byte blueValue) input) {

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

			Error? validationError = AllianceData.Color.GetColorSimilarityError(colorDifference, otherAlliance.Name.InputObject);

			validationErrors.AddIfNotNull(validationError);
		}

		return validationErrors.ToReadOnly();
	}

}