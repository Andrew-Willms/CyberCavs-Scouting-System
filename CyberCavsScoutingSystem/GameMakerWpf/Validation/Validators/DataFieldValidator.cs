using System.Linq;
using System.Collections.Generic;
using GameMakerWpf.Domain;
using GameMakerWpf.Validation.Data;
using UtilitiesLibrary;
using UtilitiesLibrary.Validation;
using Error = UtilitiesLibrary.Validation.Errors.ValidationError<CCSSDomain.ErrorSeverity>;

namespace GameMakerWpf.Validation.Validators; 



public static class DataFieldValidator {
	
	public static (Optional<string>, ReadOnlyList<Error>) NameConverter(string inputString) {

		NullInputObjectInConverterException.ThrowIfNull(inputString);

		return (inputString, ReadOnlyList<Error>.Empty);
	}

	public static (Optional<string>, ReadOnlyList<Error>) NameInverter(string name) {

		NullInputObjectInInverterException.ThrowIfNull(name);

		return (name, ReadOnlyList<Error>.Empty);
	}



	public static ReadOnlyList<Error> NameValidator_Length(string name) {

		return name.Length switch {
			<= DataFieldValidationData.Name.Length.LowerErrorThreshold => new(DataFieldValidationData.Name.Length.TooShortError),
			<= DataFieldValidationData.Name.Length.LowerWarningThreshold => new(DataFieldValidationData.Name.Length.TooShortWarning),
			<= DataFieldValidationData.Name.Length.LowerAdvisoryThreshold => new(DataFieldValidationData.Name.Length.TooShortAdvisory),
			>= DataFieldValidationData.Name.Length.UpperErrorThreshold => new(DataFieldValidationData.Name.Length.TooLongError),
			>= DataFieldValidationData.Name.Length.UpperWarningThreshold => new(DataFieldValidationData.Name.Length.TooLongWarning),
			>= DataFieldValidationData.Name.Length.UpperAdvisoryThreshold => new(DataFieldValidationData.Name.Length.TooLongAdvisory),
			_ => ReadOnlyList<Error>.Empty
		};
	}

	public static ReadOnlyList<Error> NameValidator_Uniqueness(string name,
		IEnumerable<DataFieldEditingData> otherDataFields) {

		return otherDataFields
			.Where(otherDataField => otherDataField.Name.OutputObject.HasValue)
			.Any(otherDataField => otherDataField.Name.OutputObject.Value == name)

			? new(DataFieldValidationData.Name.GetDuplicateNameError(name))
			: ReadOnlyList<Error>.Empty;
	}

}