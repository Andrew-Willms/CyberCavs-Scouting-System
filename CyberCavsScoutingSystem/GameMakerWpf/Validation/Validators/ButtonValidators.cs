using CCSSDomain;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Conversion;
using UtilitiesLibrary.Validation.Errors;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

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

	public static ReadOnlyList<Error> DataFieldNameValidator_DataFieldOfNameExists(string name, IInput<string, ErrorSeverity> _,
		IEnumerable<DataFieldEditor> dataFields) {

		return dataFields.Any(otherDataField => otherDataField.Name.OutputObject.Value == name)
			? AllianceValidationData.Name.GetDuplicateNameError(name).ReadOnlyListify()
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
			>= ButtonValidationData.ButtonTextLength.UpperErrorThreshold => AllianceValidationData.Name.Length.TooLongError.ReadOnlyListify(),
			>= ButtonValidationData.ButtonTextLength.UpperWarningThreshold => AllianceValidationData.Name.Length.TooLongWarning.ReadOnlyListify(),
			>= ButtonValidationData.ButtonTextLength.UpperAdvisoryThreshold => AllianceValidationData.Name.Length.TooLongAdvisory.ReadOnlyListify(),
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