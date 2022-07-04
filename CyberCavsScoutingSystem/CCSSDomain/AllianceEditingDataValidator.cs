using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Validation;
using WPFUtilities.Extensions;

namespace CCSSDomain;

public static class AllianceEditingDataValidator {

	public static (string?, ReadOnlyList<ValidationError<ErrorSeverity>>) NameConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return inputString.Length > 0
			? (inputString, ReadOnlyList<ValidationError<ErrorSeverity>>.Empty)
			: (null, new(new ValidationError<ErrorSeverity>("Name Can't be Empty", ErrorSeverity.Error, "Test")));
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_EndsWithAlliance(string name) {

		if (name.EndsWith(" Alliance")) {
			return ReadOnlyList<ValidationError<ErrorSeverity>>.Empty;
		}

		return new(new ValidationError<ErrorSeverity> ("Does not end in \"Alliance\"", ErrorSeverity.Advisory,
			"Typically alliance names should follow the format \"{Colour} Alliance\""));
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Length(string name) {

		//Todo extract this magic numbers.
		return name.Length switch {
			> 30 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Warning, "This alliance name is very long.")),
			> 20 => new(new ValidationError<ErrorSeverity>("Long Name", ErrorSeverity.Advisory, "This alliance name is rather long.")),
			_ => ReadOnlyList<ValidationError<ErrorSeverity>>.Empty
		};
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return (from allianceEditingData in otherAlliances
			where allianceEditingData.Name.TargetObject == name
			select new ValidationError<ErrorSeverity>("Duplicate Name", ErrorSeverity.Error,
				$"The name of this alliance is identical to that of the {allianceEditingData.Name.InputString}")).ToList().ToReadOnly();
	}



	public static (byte, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorValueValidator(string inputString) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		byte byteValue = 0;

		string invalidCharacters = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

		if (invalidCharacters.Length > 0) {
			newErrors.Add(new("Invalid Characters", ErrorSeverity.Error, "Some error text."));

		} else {

			try {
				byteValue = byte.Parse(inputString);

			} catch (Exception ex) {

				// It really should be an overflow exception but check anyway.
				if (ex is OverflowException) {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a maximum of 255"));

				} else {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a minimum of 0"));
				}
			}

		}

		return (byteValue, newErrors.ToReadOnly());
	}



	public static (Color, ReadOnlyList<ValidationError<ErrorSeverity>>) ColorConverter
		(byte redValue, byte greenValue, byte blueValue) {

		return (Color.FromRgb(redValue, greenValue, blueValue), ReadOnlyList<ValidationError<ErrorSeverity>>.Empty);
	}

	public static ReadOnlyList<ValidationError<ErrorSeverity>> ColorCovalidator_Uniqueness(Color color,
		IEnumerable<AllianceEditingData> otherAlliances) {

		List<ValidationError<ErrorSeverity>> validationErrors = new();

		// TODO: extract magic numbers
		foreach (AllianceEditingData allianceEditingData in otherAlliances) {

			if (color.Difference(allianceEditingData.AllianceColor.TargetObject) == 0) {
				validationErrors.Add(new("Colors Identical", ErrorSeverity.Warning,
					$"The color of this alliance are identical to that of the {allianceEditingData.Name.InputString}"));
			}

			if (color.Difference(allianceEditingData.AllianceColor.TargetObject) < 10) {
				validationErrors.Add(new("Colors Very Close", ErrorSeverity.Warning,
					$"The color of this alliance are very similar to that of the {allianceEditingData.Name.InputString}"));
			}

			if (color.Difference(allianceEditingData.AllianceColor.TargetObject) < 30) {
				validationErrors.Add(new("Colors Very Close", ErrorSeverity.Advisory,
					$"The color of this alliance are similar to that of the {allianceEditingData.Name.InputString}"));
			}

		}

		return validationErrors.ToReadOnly();
	}

}