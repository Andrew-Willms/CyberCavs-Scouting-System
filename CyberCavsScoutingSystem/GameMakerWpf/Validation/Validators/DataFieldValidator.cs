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
using GameMakerWpf.Domain.Editors.DataFieldEditors;

namespace GameMakerWpf.Validation.Validators;



public static class DataFieldValidator {

	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Name.Length.LowerErrorThreshold => DataFieldValidationData.Name.Length.TooShortError.ReadOnlyListify(),
			>= DataFieldValidationData.Name.Length.UpperErrorThreshold => DataFieldValidationData.Name.Length.TooLongError.ReadOnlyListify(),
			>= DataFieldValidationData.Name.Length.UpperWarningThreshold => DataFieldValidationData.Name.Length.TooLongWarning.ReadOnlyListify(),
			>= DataFieldValidationData.Name.Length.UpperAdvisoryThreshold => DataFieldValidationData.Name.Length.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}

	public static ReadOnlyList<Error> NameValidator_Uniqueness(string name, IEnumerable<DataFieldEditor> dataFields) {

		return dataFields.Multiple(x => x.Name.OutputObject.HasValue && x.Name.OutputObject.Value == name)
			? DataFieldValidationData.Name.GetDuplicateNameError(name).ReadOnlyListify()
			: ReadOnlyList.Empty;
	}
}

public static class SelectionDataFieldValidator {
	
	public static (Optional<string>, ReadOnlyList<Error>) OptionNameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) OptionNameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> OptionNameValidator_Uniqueness(string name, IEnumerable<SingleInput<string, string, ErrorSeverity>> optionNames) {

		return optionNames.Multiple(x => x.OutputObject.HasValue && x.OutputObject.Value == name)
			? DataFieldValidationData.Name.GetDuplicateNameError(name).ReadOnlyListify()
			: ReadOnlyList.Empty;
	}

	public static ReadOnlyList<Error> OptionNameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Option.LowerErrorThreshold => DataFieldValidationData.Name.Length.TooShortError.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperErrorThreshold => DataFieldValidationData.Name.Length.TooLongError.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperWarningThreshold => DataFieldValidationData.Name.Length.TooLongWarning.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperAdvisoryThreshold => DataFieldValidationData.Name.Length.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}

}

public static class IntegerDataFieldValidator {

	public static (Optional<int>, ReadOnlyList<Error>) IntegerValueConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return StringConversion.ToInt(inputString, DataFieldValidationData.IntegerValue.ConversionErrorSet);
	}

	public static (Optional<string>, ReadOnlyList<Error>) IntegerValueInverter(int integerValue) {

		return (integerValue.ToString().Optionalize(), ReadOnlyList.Empty);
	}

}