﻿using System.Collections.Generic;
using System.Linq;
using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Conversion;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary.Validation;
using UtilitiesLibrary.Validation.Inputs;
using UtilitiesLibrary.Collections;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary.Optional;
using static System.String;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<GameMakerWpf.Domain.ErrorSeverity>;

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

public static class TextDataFieldValidator {

	public static (Optional<string>, ReadOnlyList<Error>) DefaultValueConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DefaultValueInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
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
			? DataFieldValidationData.Option.GetDuplicateNameError(name).ReadOnlyListify()
			: ReadOnlyList.Empty;
	}

	public static ReadOnlyList<Error> OptionNameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Option.LowerErrorThreshold => DataFieldValidationData.Option.TooShortError.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperErrorThreshold => DataFieldValidationData.Option.TooLongError.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperWarningThreshold => DataFieldValidationData.Option.TooLongWarning.ReadOnlyListify(),
			>= DataFieldValidationData.Option.UpperAdvisoryThreshold => DataFieldValidationData.Option.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}

	public static (Optional<string>, ReadOnlyList<Error>) InitialValueNameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) InitialValueNameInverter(string initialValue) {

		return (initialValue, ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> InitialValueValidator(string initialValue, IEnumerable<SingleInput<string, string, ErrorSeverity>> optionNames) {

		if (initialValue == Empty) {
			return ReadOnlyList.Empty;
		}

		return optionNames.SelectIfHasValue(x => x.OutputObject).Contains(initialValue)
			? ReadOnlyList.Empty
			: DataFieldValidationData.Option.GetInvalidInitialValueError(initialValue).ReadOnlyListify();
	}

}