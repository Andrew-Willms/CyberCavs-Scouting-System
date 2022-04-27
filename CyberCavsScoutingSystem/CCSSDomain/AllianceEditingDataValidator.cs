using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using WPFUtilities;

namespace CCSSDomain; 

public class AllianceEditingDataValidator {

	private AllianceEditingData EditingData { get; }

	public AllianceEditingDataValidator(AllianceEditingData editingData) {

		EditingData = editingData;
	}




	public (int, ReadOnlyCollection<ValidationError<ErrorSeverity>>) ColorValueValidator(string inputString) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		int intValue = 0;

		string invalidCharacters = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

		if (invalidCharacters.Length > 0) {
			newErrors.Add(new("Invalid Characters", ErrorSeverity.Error, "asdf"));

		} else {

			try {
				intValue = int.Parse(inputString);

			} catch (Exception ex) {

				// It really should be an overflow exception but check anyway.
				if (ex is OverflowException) {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a maximum of 255"));

				} else {
					newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a minimum of 0"));;
				}
			}

		}

		if (intValue > 255) {
			newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a maximum of 255"));
		}

		if (intValue < 0) {
			newErrors.Add(new("Color Value Out of Range", ErrorSeverity.Error, "The color value can be a minimum of 0"));
		}

		return (intValue, newErrors.AsReadOnly());
	}

	public (string, ReadOnlyCollection<ValidationError<ErrorSeverity>>) NameValidator(string inputString) {

		return (inputString, new List<ValidationError<ErrorSeverity>>().AsReadOnly());
	}
}