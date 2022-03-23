using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities;

namespace CCSSDomain;

public class GameEditingDataValidator {

	private GameEditingData EditingData { get; }

	public GameEditingDataValidator(GameEditingData editingData) {

		EditingData = editingData;

	}



	// TODO: this function needs a bunch of work. The code is pretty ugly.
	public void YearValueConverter(ref int value, string inputString, out ReadOnlyCollection<ValidationError<ErrorSeverity>> errors) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		value = default;

		try {

			value = int.Parse(inputString);

			if (value < 0) {
				newErrors.Add(new("Unconventional Year Specified", ErrorSeverity.Warning, "The year specified is negative. It seems unlikely this is intentional."));
			
			// TODO: move the magic number elsewhere when I have a place for such resources.
			} else if (value < 1992) {
				newErrors.Add(new("Unconventional Year", ErrorSeverity.Advisory, "The year specified is before the year of the first FRC event."));
			}

			else if (value > DateTime.Now.Year + 1) {
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

		errors = newErrors.AsReadOnly();
	}


	public void TestIntValueConverter(ref int intValue, string inputString, out ReadOnlyCollection<ValidationError<ErrorSeverity>> errors) {

		List<ValidationError<ErrorSeverity>> newErrors = new();
		intValue = default;

		string invalidCharacters = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

		if (invalidCharacters.Length > 0) {
			newErrors.Add(new("Invalid Characters", ErrorSeverity.Error));

		} else {

			try {
				intValue = int.Parse(inputString);

			} catch (Exception ex) {

				// It really should be an overflow exception but check anyway.
				if (ex is OverflowException) {
					newErrors.Add(new("Number Too Large", ErrorSeverity.Error));

				} else {
					newErrors.Add(new("Unknown Error", ErrorSeverity.Error));
				}
			}

		}

		errors = newErrors.AsReadOnly();
	}
}
