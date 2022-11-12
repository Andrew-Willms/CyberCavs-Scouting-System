using System.Linq;
using System.Collections.Generic;
using CCSSDomain;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.MiscExtensions;
using GameMakerWpf.Domain.Editors;

namespace GameMakerWpf.Validation.Validators;



public static class DataFieldValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Name.Length.LowerErrorThreshold => new(DataFieldValidationData.Name.Length
				.TooShortError),
			>= DataFieldValidationData.Name.Length.UpperErrorThreshold => new(DataFieldValidationData.Name.Length
				.TooLongError),
			>= DataFieldValidationData.Name.Length.UpperWarningThreshold => new(DataFieldValidationData.Name.Length
				.TooLongWarning),
			>= DataFieldValidationData.Name.Length.UpperAdvisoryThreshold => new(DataFieldValidationData.Name.Length
				.TooLongAdvisory),
			_ => ReadOnlyList<Error>.Empty
		};
	}

	public static ReadOnlyList<Error> NameValidator_Uniqueness(string name, IInput<string, ErrorSeverity> validatee,
		IEnumerable<DataFieldEditor> dataFields) {

		if (dataFields.Where(dataField => dataField.Name.OutputObject.HasValue)
		    .Select(dataField => dataField.Name.OutputObject.Value)
		    .OnlyOne(name)) {

			return ReadOnlyList<Error>.Empty;
		}

		return validatee.OutputObject.Value == name
			? new(DataFieldValidationData.Name.GetDuplicateNameError(name))
			: ReadOnlyList<Error>.Empty;
	}
}

public static class SelectionDataFieldValidator {
	
	public static (Optional<string>, ReadOnlyList<Error>) OptionNameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) OptionNameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList<Error>.Empty);
	}

	public static ReadOnlyList<Error> OptionNameValidator_Uniqueness(string name, IInput<string, ErrorSeverity> validatee,
		IEnumerable<SingleInput<string, string, ErrorSeverity>> optionNames) {

		return optionNames.Any(otherOptionName => otherOptionName.OutputObject == name)
			? new(DataFieldValidationData.Name.GetDuplicateNameError(name))
			: ReadOnlyList<Error>.Empty;
	}

	public static ReadOnlyList<Error> OptionNameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Option.LowerErrorThreshold => new(DataFieldValidationData.Name.Length.TooShortError),
			>= DataFieldValidationData.Option.UpperErrorThreshold => new(DataFieldValidationData.Name.Length.TooLongError),
			>= DataFieldValidationData.Option.UpperWarningThreshold => new(DataFieldValidationData.Name.Length.TooLongWarning),
			>= DataFieldValidationData.Option.UpperAdvisoryThreshold => new(DataFieldValidationData.Name.Length.TooLongAdvisory),
			_ => ReadOnlyList<Error>.Empty
		};
	}

}

public static class IntegerDataFieldValidator {

	public static (Optional<int>, ReadOnlyList<Error>) IntegerValueConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, DataFieldValidationData.IntegerValue.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) IntegerValueInverter(int integerValue) {

		return (integerValue.ToString().Optionalize(), ReadOnlyList<Error>.Empty);
	}

}