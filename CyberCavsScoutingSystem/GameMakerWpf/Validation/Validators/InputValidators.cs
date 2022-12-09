using System.Collections.Generic;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Optional;
using UtilitiesLibrary.Validation;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators;



public static class InputValidators {
	
	public static (Optional<string>, ReadOnlyList<Error>) DataFieldNameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) DataFieldNameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name.Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> DataFieldNameValidator_DataFieldOfNameExists(string name, IEnumerable<DataFieldEditor> dataFields) {

		return dataFields.None(x => x.Name.OutputObject.HasValue && x.Name.OutputObject.Value == name)
			? InputValidationData.DataField.DataFieldDoesNotExistError.ReadOnlyListify()
			: ReadOnlyList.Empty;
	}



	public static (Optional<string>, ReadOnlyList<Error>) InputTextConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString.Optionalize(), ReadOnlyList.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) InputTextInverter(string inputText) {

		NullInputObjectInInverterException.ThrowIfNull(inputText);

		return (inputText.Optionalize(), ReadOnlyList.Empty);
	}

	public static ReadOnlyList<Error> InputTextValidator_Length(string inputText) {

		return inputText.Length switch {
			>= InputValidationData.InputTextLength.UpperErrorThreshold => InputValidationData.InputTextLength.TooLongError.ReadOnlyListify(),
			>= InputValidationData.InputTextLength.UpperWarningThreshold => InputValidationData.InputTextLength.TooLongWarning.ReadOnlyListify(),
			>= InputValidationData.InputTextLength.UpperAdvisoryThreshold => InputValidationData.InputTextLength.TooLongAdvisory.ReadOnlyListify(),
			_ => ReadOnlyList.Empty
		};
	}

}