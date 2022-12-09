using System.Windows.Media;
using System.Collections.Generic;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using GameMakerWpf.Domain.Editors;
using UtilitiesLibrary.Optional;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators;



public static class AllianceValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_EndsWithAlliance(string name) {

		return name.EndsWith(AllianceValidationData.Name.ShouldEndWith)
			? ReadOnlyList.Empty
			: AllianceValidationData.Name.DoesNotEndWithCorrectSequenceError.ReadOnlyListify();
	}

	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= AllianceValidationData.Name.Length.LowerErrorThreshold => AllianceValidationData.Name.Length.TooShortError.ReadOnlyListify(),
			<= AllianceValidationData.Name.Length.LowerWarningThreshold => AllianceValidationData.Name.Length.TooShortWarning.ReadOnlyListify(),
			<= AllianceValidationData.Name.Length.LowerAdvisoryThreshold => AllianceValidationData.Name.Length.TooShortAdvisory.ReadOnlyListify(),
			>= AllianceValidationData.Name.Length.UpperErrorThreshold => AllianceValidationData.Name.Length.TooLongError.ReadOnlyListify(),
			>= AllianceValidationData.Name.Length.UpperWarningThreshold => AllianceValidationData.Name.Length.TooLongWarning.ReadOnlyListify(),
			>= AllianceValidationData.Name.Length.UpperAdvisoryThreshold => AllianceValidationData.Name.Length.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}

	public static ReadOnlyList<Error> NameValidator_Uniqueness(string name, IEnumerable<AllianceEditor> alliances) {

		return alliances.Multiple(x => x.Name.OutputObject.HasValue && x.Name.OutputObject.Value == name)
			? AllianceValidationData.Name.GetDuplicateNameError(name).ReadOnlyListify()
			: ReadOnlyList.Empty;
	}



	public static (Optional<byte>, ReadOnlyList<Error>) ColorComponentConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToByte(inputString, AllianceValidationData.Color.Component.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) ColorComponentInverter(byte colourComponentValue) {

		return (colourComponentValue.ToString().Optionalize(), ReadOnlyList.Empty);
	}



	public static (Optional<Color>, ReadOnlyList<Error>) ColorConverter((byte redValue, byte greenValue, byte blueValue) input) {

		return (Color.FromRgb(input.redValue, input.greenValue, input.blueValue).Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<(byte redValue, byte greenValue, byte blueValue)> invertedValues, ReadOnlyList<Error>) ColorInverter
		(Color color) {

		return ((color.R, color.G, color.B).Optionalize(), ReadOnlyList.Empty);
	}



	public static ReadOnlyList<Error> ColorCovalidator_Uniqueness(Color color, IEnumerable<AllianceEditor> otherAlliances) {

		List<Error> validationErrors = new();

		foreach (AllianceEditor otherAlliance in otherAlliances) {

			Optional<Color> allianceColorOption = otherAlliance.AllianceColor.OutputObject;

			if (allianceColorOption.HasValue == false) {
				continue;
			}

			double colorDifference = color.Difference(allianceColorOption.Value);

			Optional<Error> validationError = AllianceValidationData.Color.GetColorSimilarityError(colorDifference, otherAlliance.Name.InputObject);

			validationErrors.AddIfHasValue(validationError);
		}

		return validationErrors.ToReadOnly();
	}

}