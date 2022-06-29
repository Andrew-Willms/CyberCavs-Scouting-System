using System;
using Math = System.Math;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using WPFUtilities;
using WPFUtilities.Validation;

namespace CCSSDomain;

public static class AllianceEditingDataValidator {

	public static (string, ReadOnlyList<ValidationError<ErrorSeverity>>) NameConverter(string inputString) {

		return (inputString, new());
	}

	// TODO: replace ValidationError? with Optional<ValidationError>.
	public static ValidationError<ErrorSeverity>? NameValidator_EndsWithAlliance(string name) {

		if (name.EndsWith(" Alliance")) {
			return null;
		}

		return new("Does not end in \"Alliance\"", ErrorSeverity.Advisory,
			"Typically alliance names should follow the format \"{Colour} Alliance\"");
	}

	public static ValidationError<ErrorSeverity>? NameValidator_Length(string name) {

		//Todo extract this magic numbers.

		return name.Length switch {
			> 30 => new("Long Name", ErrorSeverity.Warning, "This alliance name is very long."),
			> 20 => new("Long Name", ErrorSeverity.Advisory, "This alliance name is rather long."),
			_ => null
		};
	}

	public static ValidationError<ErrorSeverity>? NameValidator_Duplicate(string name, IEnumerable<AllianceEditingData> alliances) {

		return alliances.Count(x => x.Name.TargetObject == name) switch {

			0 => throw new ArgumentException($"No alliance has the name \"{name}\"."),

			1 => null,

			> 1 => new("Duplicate Name", ErrorSeverity.Error, $"Multiple Alliances share the name \"{name}\"."),

			_ => throw new Exception("Uh... the enumerable had a negative count. How did you manage that?")
		};
	}



	public static (byte, ObservableCollection<ValidationError<ErrorSeverity>>) ColorValueValidator(string inputString) {

		ObservableCollection<ValidationError<ErrorSeverity>> newErrors = new();
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

		return (byteValue, newErrors);
	}



	public static (Color, ObservableCollection<ValidationError<ErrorSeverity>>) ColorCovalidator
		(in ReadOnlyDictionary<string, IStringInput<ErrorSeverity>> inputComponents) {

		if (inputComponents[nameof(Color.R)] is not StringInput<byte, ErrorSeverity> redValueInput) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.R)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (inputComponents[nameof(Color.G)] is not StringInput<byte, ErrorSeverity> greenValueInput) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.G)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (inputComponents[nameof(Color.B)] is not StringInput<byte, ErrorSeverity> blueValueInput) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(Color.B)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		Color color = Color.FromRgb(redValueInput.TargetObject, greenValueInput.TargetObject, blueValueInput.TargetObject);
		List<ValidationError<ErrorSeverity>> validationErrors = new();

		if (EditingData.Alliances is null || AllianceEditingData is null) {
			return (color, validationErrors.AsReadOnly());
		}

		foreach (AllianceEditingData allianceEditingData in EditingData.Alliances.Where(x => x != AllianceEditingData)) {

			bool test1 = allianceEditingData is null;
			bool test2 = allianceEditingData.AllianceColor is null;
			bool test3 = AllianceEditingData is null;
			bool test4 = AllianceEditingData.AllianceColor is null;

			int colorDifference = ColorDifference(allianceEditingData.AllianceColor.TargetObject,
				AllianceEditingData.AllianceColor.TargetObject);

			switch (colorDifference) {

				case 0:
					validationErrors.Add(new("Colors Identical", ErrorSeverity.Warning,
						$"The color of this alliance are identical to that of the {allianceEditingData.Name.InputString}"));
					break;

				case < 10:
					validationErrors.Add(new("Colors Very Close", ErrorSeverity.Warning,
						$"The color of this alliance are very similar to that of the {allianceEditingData.Name.InputString}"));
					break;

				case < 30:
					validationErrors.Add(new("Colors Very Close", ErrorSeverity.Advisory,
						$"The color of this alliance are similar to that of the {allianceEditingData.Name.InputString}"));
					break;
			}
		}

		return (color, validationErrors.AsReadOnly());
	}

	private static int ColorDifference(Color color1, Color color2) {

		return Math.Abs(color1.R - color2.R) + System.Math.Abs(color1.G - color2.G) + Math.Abs(color1.B - color2.B);
	}

}