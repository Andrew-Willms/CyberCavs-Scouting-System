using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WPFUtilities;

namespace CCSSDomain;

public class GameEditingDataValidator {

	private GameEditingData EditingData { get; }

	public GameEditingDataValidator(GameEditingData editingData) {

		EditingData = editingData;

	}



	// TODO: this function needs a bunch of work. The code is pretty ugly.
	public (int, ReadOnlyCollection<ValidationError<ErrorSeverity>>) YearValidator(string inputString) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		int newValue = 0;

		try {

			newValue = int.Parse(inputString);

			if (newValue < 0) {
				newErrors.Add(new("Unconventional Year Specified", ErrorSeverity.Warning, "The year specified is negative. It seems unlikely this is intentional."));
			
			// TODO: move the magic number elsewhere when I have a place for such resources.
			} else if (newValue < 1992) {
				newErrors.Add(new("Unconventional Year", ErrorSeverity.Advisory, "The year specified is before the year of the first FRC event."));
			}

			else if (newValue > DateTime.Now.Year + 1) {
				newErrors.Add(new("Year in Future", ErrorSeverity.Advisory, "The year specified is more than a year in the future."));
			}

		} catch (Exception ex) {

			switch (ex) {

				case FormatException: {
					string invalidCharacters = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

					if (inputString.Length == 0) {
						newErrors.Add(new("No Valid Characters", ErrorSeverity.Error, "No number was provided."));

					} else if (invalidCharacters.Length == inputString.Length) {
						newErrors.Add(new("No Valid Characters", ErrorSeverity.Error, "The text provided does not contain numerical characters."));

					} else {
						(string errorName, string errorDescription) = invalidCharacters.Length > 0
							? ("Invalid Characters", $"The text provided contains the invalid characters \"{invalidCharacters}\".")
							: ("Unknown Format Exception", "The text provided caused an unknown format exception.");

						newErrors.Add(new(errorName, ErrorSeverity.Error, errorDescription));
					}

					break;
				}

				case OverflowException: {
					string errorDescription = inputString.Contains('-')
						? $"The year is too high, the maximum value is {int.MaxValue}."
						: $"The year is too low, the maximum value is {int.MinValue}.";

					newErrors.Add(new("Number Too Large", ErrorSeverity.Error, errorDescription));
					break;
				}

				default:
					newErrors.Add(new("Unknown Error", ErrorSeverity.Error));
					break;
			}
		}

		return (newValue, newErrors.AsReadOnly());
	}


	public (VersionNumber, ReadOnlyCollection<ValidationError<ErrorSeverity>>) VersionCovalidator
		(in VersionNumber? value, in ReadOnlyDictionary<string, IStringInput<ErrorSeverity>> inputComponents) {

		var majorNumberInput = inputComponents[nameof(VersionNumber.MajorNumber)] as StringInput<int, ErrorSeverity>;
		var minorNumberInput = inputComponents[nameof(VersionNumber.MinorNumber)] as StringInput<int, ErrorSeverity>;
		var patchNumberInput = inputComponents[nameof(VersionNumber.PatchNumber)] as StringInput<int, ErrorSeverity>;

		if (majorNumberInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(VersionNumber.MajorNumber)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (minorNumberInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(VersionNumber.MinorNumber)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		if (patchNumberInput is null) {
			throw new ArgumentException($"{nameof(inputComponents)}[{nameof(VersionNumber.PatchNumber)}] is null or cannot be converted to a {nameof(StringInput<int, ErrorSeverity>)}<{typeof(int)}, {nameof(ErrorSeverity)}>");
		}

		VersionNumber version = new(majorNumberInput.TargetObject, minorNumberInput.TargetObject, patchNumberInput.TargetObject);
		List<ValidationError<ErrorSeverity>> validationErrors = new();

		if (version.MajorNumber + version.MinorNumber + version.PatchNumber > 10) {
			validationErrors.Add(new("(test) total version numbers > 10", ErrorSeverity.Advisory, "something here"));
		}

		return (version, validationErrors.AsReadOnly());
	}




	public (int, ReadOnlyCollection<ValidationError<ErrorSeverity>>) TestIntValueConverter(string inputString) {

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
					newErrors.Add(new("Number Too Large", ErrorSeverity.Error, "asdf"));

				} else {
					newErrors.Add(new("Unknown Error", ErrorSeverity.Error, "asdf"));
				}
			}

		}

		return (intValue, newErrors.AsReadOnly());
	}



	public (string, ReadOnlyCollection<ValidationError<ErrorSeverity>>) NameValidator(string inputString) {
		
		List<ValidationError<ErrorSeverity>> newErrors = new();

		// TODO: Move magic numbers to ValidationErrorParameters data or something.
		if (inputString.Length > 40) {

			newErrors.Add(new("Large Name", ErrorSeverity.Advisory, "The name specified is abnormally large"));
		}

		return (inputString, newErrors.AsReadOnly());
	}

	public (string, ReadOnlyCollection<ValidationError<ErrorSeverity>>) DescriptionValidator(string inputString) {

		return (inputString, new List<ValidationError<ErrorSeverity>>().AsReadOnly());
	}
}
