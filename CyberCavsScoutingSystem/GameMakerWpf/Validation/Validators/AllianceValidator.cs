using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using CCSSDomain;
using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;

namespace GameMakerWpf.Validation.Validators;



public static class AllianceValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(AllianceValidationData.Name.ShouldEndWith)
			? ReadOnlyList<Error>.Empty
			: new(AllianceValidationData.Name.DoesNotEndWithCorrectSequenceError);
	}

	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceValidationData.Name.Length.LowerErrorThreshold => new(AllianceValidationData.Name.Length.TooShortError),
			<= AllianceValidationData.Name.Length.LowerWarningThreshold => new(AllianceValidationData.Name.Length.TooShortWarning),
			<= AllianceValidationData.Name.Length.LowerAdvisoryThreshold => new(AllianceValidationData.Name.Length.TooShortAdvisory),
			>= AllianceValidationData.Name.Length.UpperErrorThreshold => new(AllianceValidationData.Name.Length.TooLongError),
			>= AllianceValidationData.Name.Length.UpperWarningThreshold => new(AllianceValidationData.Name.Length.TooLongWarning),
			>= AllianceValidationData.Name.Length.UpperAdvisoryThreshold => new(AllianceValidationData.Name.Length.TooLongAdvisory),
			_ => ReadOnlyList<Error>.Empty
		};
	}

	public static ReadOnlyList<Error> NameValidator_Uniqueness(string name, IInput<string, ErrorSeverity> validatee,
		IEnumerable<AllianceEditingData> alliances) {

		return alliances
			.Where(otherAlliance => otherAlliance.Name != validatee && otherAlliance.Name.OutputObject.HasValue)
			.Any(otherAlliance => otherAlliance.Name.OutputObject.Value == name)

			? new(AllianceValidationData.Name.GetDuplicateNameError(name))
			: ReadOnlyList<Error>.Empty;
	}



	public static (Optional<byte>, ReadOnlyList<Error>) ColorComponentConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		(Optional<byte>, ReadOnlyList<Error>) test = StringConversion.ToByte(inputString, AllianceValidationData.Color.Component.ConversionErrorSet);
		ReadOnlyList<Error> test2 = test.Item2;

		return StringConversion.ToByte(inputString, AllianceValidationData.Color.Component.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) ColorComponentInverter(byte colourComponentValue) {

		return (colourComponentValue.ToString().Optionalize(), ReadOnlyList<Error>.Empty);
	}



	public static (Optional<Color>, ReadOnlyList<Error>) ColorConverter((byte redValue, byte greenValue, byte blueValue) input) {

		return (Color.FromRgb(input.redValue, input.greenValue, input.blueValue).Optionalize(), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<(byte redValue, byte greenValue, byte blueValue)> invertedValues, ReadOnlyList<Error>) ColorInverter
		(Color color) {

		return ((color.R, color.G, color.B).Optionalize(), ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> ColorCovalidator_Uniqueness(Color color, IInput<Color, ErrorSeverity> validatee,
		IEnumerable<AllianceEditingData> otherAlliances) {

		List<Error> validationErrors = new();

		foreach (AllianceEditingData otherAlliance in otherAlliances) {

			if (otherAlliance.AllianceColor == validatee) {
				continue;
			}

			Optional<Color> allianceColorOption = otherAlliance.AllianceColor.OutputObject;

			if (allianceColorOption.HasValue == false) {
				continue;
			}

			int colorDifference = color.Difference(allianceColorOption.Value);

			Optional<Error> validationError = AllianceValidationData.Color.GetColorSimilarityError(colorDifference, otherAlliance.Name.InputObject);

			validationErrors.AddIfHasValue(validationError);
		}

		return validationErrors.ToReadOnly();
	}

}