using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Extensions;
using WPFUtilities.Validation.Delegates;
using WPFUtilities.Validation.Errors;

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

		if (name.EndsWith(" Alliance")) {
			return ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
		}

		return new(new ValidationError<ErrorSeverity>("Does not end in \"Alliance\"", ErrorSeverity.Advisory,
			"Typically alliance names should follow the format \"{Colour} Alliance\""));
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Length(string name) {

		//Todo extract this magic numbers.
		return name.Length switch {
			0 => new(new ValidationError<ErrorSeverity>("Empty Name", ErrorSeverity.Warning, "This alliance cannot have a blank name.")),
			> 30 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Warning, "This alliance name is very long.")),
			> 20 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Advisory, "This alliance name is rather long.")),
			_ => ReadOnlyList<ValidationError<ErrorSeverity>>.Empty
		};
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return (from allianceEditingData in otherAlliances
				where allianceEditingData.Name.OutputObject == name
				select new ValidationError<ErrorSeverity>("Duplicate Name", ErrorSeverity.Error,
					$"The name of this alliance is identical to that of the {allianceEditingData.Name.InputObject}")).ToList().ToReadOnly();
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

		// TODO: extract magic numbers
		foreach (AllianceEditingData allianceEditingData in otherAlliances) {

			if (color.Difference(allianceEditingData.AllianceColor.OutputObject) == 0) {
				validationErrors.Add(new("Colors Identical", ErrorSeverity.Warning,
					$"The color of this alliance are identical to that of the {allianceEditingData.Name.InputObject}"));
			}

			if (color.Difference(allianceEditingData.AllianceColor.OutputObject) < 10) {
				validationErrors.Add(new("Colors Very Close", ErrorSeverity.Warning,
					$"The color of this alliance are very similar to that of the {allianceEditingData.Name.InputObject}"));
			}

			if (color.Difference(allianceEditingData.AllianceColor.OutputObject) < 30) {
				validationErrors.Add(new("Colors Very Close", ErrorSeverity.Advisory,
					$"The color of this alliance are similar to that of the {allianceEditingData.Name.InputObject}"));
			}

		}

		return validationErrors.ToReadOnly();
	}

}