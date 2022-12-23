using CCSSDomain;
using System.Collections.Generic;
using System.Globalization;
using CCSSDomain.GameSpecification;
using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Conversion;
using UtilitiesLibrary.Validation.Errors;
using UtilitiesLibrary.Optional;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators;



public static class ButtonValidators {
	
	public static (Optional<string>, ReadOnlyList<Error>) DataFieldNameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DataFieldNameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> DataFieldNameValidator_DataFieldOfNameExists(string name, IEnumerable<DataFieldEditor> dataFields) {

		return dataFields.None(x => x.Name.OutputObject.HasValue &&
		                           x.Name.OutputObject.Value == name &&
		                           x.DataFieldType == DataField.DataFieldType.Integer)

			? ButtonValidationData.DataField.DataFieldDoesNotExistError.ReadOnlyListify()
			: ReadOnlyList.Empty;
	}



	public static (Optional<string>, ReadOnlyList<Error>) ButtonTextConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) ButtonTextInverter(string buttonText) {

		NullInputObjectInInverterException.ThrowIfNull(buttonText);

		return (buttonText.Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> ButtonTextValidator_Length(string buttonText) {

		return buttonText.Length switch {
			>= ButtonValidationData.ButtonTextLength.UpperErrorThreshold => ButtonValidationData.ButtonTextLength.TooLongError.ReadOnlyListify(),
			>= ButtonValidationData.ButtonTextLength.UpperWarningThreshold => ButtonValidationData.ButtonTextLength.TooLongWarning.ReadOnlyListify(),
			>= ButtonValidationData.ButtonTextLength.UpperAdvisoryThreshold => ButtonValidationData.ButtonTextLength.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}




	public static (Optional<int>, ReadOnlyList<Error>) IncrementAmountConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, ButtonValidationData.IncrementAmount.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) IncrementAmountInverter(int colourComponentValue) {

		return (colourComponentValue.ToString().Optionalize(), ReadOnlyList.Empty);
	}



	public static (Optional<double>, ReadOnlyList<Error>) PositionConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToFloat64(inputString, ButtonValidationData.XPosition.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) PositionInverter(double xPosition) {

		return (xPosition.ToString(CultureInfo.InvariantCulture).Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> PositionValidator_BetweenZeroAndOne(double xPosition) {

		return xPosition is > 1 or < 0
			? new ValidationError<ErrorSeverity>("Off Screen", ErrorSeverity.Warning, "The value you entered is outside of the expected range of 0 to 1.").ReadOnlyListify()
			: ReadOnlyList<Error>.Empty;
	}

}