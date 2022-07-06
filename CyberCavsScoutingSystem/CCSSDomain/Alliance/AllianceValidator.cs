﻿using System;
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

	private static (string?, ValidationError<ErrorSeverity>?) NameConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		return (inputString, null);
	}

	private static (string?, ValidationError<ErrorSeverity>?) NameInverter(string name) {

		if (name is null) {
			throw new ArgumentNullException(nameof(name), "You shouldn't be able to send a null value to an inverter.");
		}

		return (name, null);
	}
	
	public static readonly ConversionPair<string, string, ErrorSeverity> NameConversionPair = new(NameConverter, NameInverter);



	public static ValidationError<ErrorSeverity>? NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(" Alliance") ? null : AllianceData.Name.DoesNotEndWithAllianceError;
	}

	public static ValidationError<ErrorSeverity>? NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceData.Name.Length.LowerErrorThreshold => AllianceData.Name.Length.TooShortError,
			<= AllianceData.Name.Length.LowerWarningThreshold => AllianceData.Name.Length.TooShortWarning,
			<= AllianceData.Name.Length.LowerAdvisoryThreshold => AllianceData.Name.Length.TooShortAdvisory,
			>= AllianceData.Name.Length.UpperErrorThreshold => AllianceData.Name.Length.TooLongError,
			>= AllianceData.Name.Length.UpperWarningThreshold => AllianceData.Name.Length.TooLongWarning,
			>= AllianceData.Name.Length.UpperAdvisoryThreshold => AllianceData.Name.Length.TooLongAdvisory,
			_ => null
		};
	}

	public static ValidationError<ErrorSeverity>? NameValidator_Uniqueness(string name,
		IEnumerable<AllianceEditingData> otherAlliances) {

		return otherAlliances.Any(otherAlliance => otherAlliance.Name.OutputObject == name)
			? AllianceData.Name.GetDuplicateNameError(name)
			: null;
	}



	private static (byte, ValidationError<ErrorSeverity>?) ColorComponentConverter(string inputString) {

		if (inputString is null) {
			throw new ArgumentNullException(nameof(inputString), "You shouldn't be able to send a null string to this validator.");
		}

		if (inputString.Length == 0) {
			return (0, new("Empty Year Field", ErrorSeverity.Error, "The year cannot have no value."));
		}

		char[] invalidCharacters = inputString.Where(x => char.IsDigit(x) == false).ToArray();

		if (invalidCharacters.Any()) {
			return (0, new("Invalid Characters", ErrorSeverity.Error,
				$"The characters \"{invalidCharacters}\" are not valid in the year field."));
		}

		if (inputString.NumericCompare(byte.MaxValue.ToString()) > 1) {
			return (0, new("Number Too Large", ErrorSeverity.Error, "Too big to convert to int."));
		}

		return (byte.Parse(inputString), null);
	}

	private static (string, ValidationError<ErrorSeverity>?) ColorComponentInverter(
		byte colourComponentValue) {

		return (colourComponentValue.ToString(), null);
	}

	public static readonly ConversionPair<byte, string, ErrorSeverity> ColorComponentConversionPair
		= new(ColorComponentConverter, ColorComponentInverter);



	private static (Color, ValidationError<ErrorSeverity>?) ColorConverter
		((byte redValue, byte greenValue, byte blueValue) input) {

		return (Color.FromRgb(input.redValue, input.greenValue, input.blueValue), null);
	}

	private static ((byte redValue, byte greenValue, byte blueValue) invertedValues, ValidationError<ErrorSeverity>?) ColorInverter
		(Color color) {

		return ((color.R, color.G, color.B), null);
	}

	public static readonly ConversionPair<Color, (byte, byte, byte), ErrorSeverity> ColorConversionPair
		= new(ColorConverter, ColorInverter);



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