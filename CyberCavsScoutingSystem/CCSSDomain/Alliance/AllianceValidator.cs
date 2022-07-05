using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Extensions;
using WPFUtilities.Validation.Delegates;
using WPFUtilities.Validation.Errors;
using CCSSDomain.Data;

namespace CCSSDomain.Alliance;



public static class AllianceValidator {

	private static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) NameConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	private static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) NameInverter(string name) {

		if (name is null) {
			throw new ArgumentNullException(nameof(name), "You shouldn't be able to send a null value to an inverter.");
		}

		return (name, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}
	
	public static readonly ConversionPair<string, string, ErrorSeverity> NameConversionPair = new(NameConverter, NameInverter);



	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(" Alliance")
			? ReadOnlyList<ValidationError<ErrorSeverity>>.Empty
			: new(AllianceData.Name.DoesNotEndWithAllianceError);
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceData.Name.Length.LowerErrorThreshold => new(AllianceData.Name.Length.TooShortError),
			<= AllianceData.Name.Length.LowerWarningThreshold => new(AllianceData.Name.Length.TooShortWarning),
			<= AllianceData.Name.Length.LowerAdvisoryThreshold => new(AllianceData.Name.Length.TooShortAdvisory),
			>= AllianceData.Name.Length.UpperErrorThreshold => new(AllianceData.Name.Length.TooLongError),
			>= AllianceData.Name.Length.UpperWarningThreshold => new(AllianceData.Name.Length.TooLongWarning),
			>= AllianceData.Name.Length.UpperAdvisoryThreshold => new(AllianceData.Name.Length.TooLongAdvisory),
			_ => ReadOnlyList<ValidationError<ErrorSeverity>>.Empty
		};
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return otherAlliances.Any(otherAlliance => otherAlliance.Name.OutputObject == name)
			? new(AllianceData.Name.GetDuplicateNameError(name))
			: ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
	}



	private static (byte, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorComponentConverter(string inputString) {

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

		if (inputString.NumericCompare(byte.MaxValue.ToString()) > 1) {
			return (0, new(new ValidationError<ErrorSeverity>("Number Too Large", ErrorSeverity.Error, "Too big to convert to int.")));
		}

		return (byte.Parse(inputString), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	private static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorComponentInverter(
		byte colourComponentValue) {

		return (colourComponentValue.ToString(), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static readonly ConversionPair<byte, string, ErrorSeverity> ColorComponentConversionPair
		= new(ColorComponentConverter, ColorComponentInverter);



	public static (Color, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorConverter
		(byte redValue, byte greenValue, byte blueValue) {

		return (Color.FromRgb(redValue, greenValue, blueValue), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static (byte redValue, byte greenValue, byte blueValue, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorInverter
		(Color color) {

		return (color.R, color.G, color.B, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	//public static ConversionPair<Color, (byte, byte, byte), ErrorSeverity> ColorConversionPair
	//	= new(ColorConverter, ColorInverter);



	public static ReadOnlyList<ValidationError<ErrorSeverity>> ColorCovalidator_Uniqueness(Color color,
		IEnumerable<AllianceEditingData> otherAlliances) {

		List<ValidationError<ErrorSeverity>> validationErrors = new();

		foreach (AllianceEditingData otherAlliance in otherAlliances) {

			int colorDifference = color.Difference(otherAlliance.AllianceColor.OutputObject);

			ValidationError<ErrorSeverity>? validationError =
				AllianceData.Color.GetColorSimilarityError(colorDifference, otherAlliance.Name.InputObject);

			validationErrors.AddIfNotNull(validationError);
		}

		return validationErrors.ToReadOnly();
	}

}