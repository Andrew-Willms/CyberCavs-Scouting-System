using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses;

public class GameEditingData {

	public GameEditingData() {

		//Version = new(VersionNumberValueConverter, new string[] { "MajorNumber", "MinorNumber", "PatchNumber" }, new string[] { "0", "0", "0" });

		//Year = new(YearValueConverter, new string[] { "" }, new string[] { "0" });

		TestInt = new(TestIntValueConverter, "42");

	}

	public int TestInt2 { get; set; }

	public readonly StringInput<VersionNumber> Version; // Cannot be initialized here because it needs to pass the instance member VersionNumberValueConverter

	public void VersionNumberValueConverter(ref VersionNumber targetObject, string propertyIdentifier, Dictionary<string, string> inputStrings,
		out List<StringInputValidationError> errors) {

			// If the propertyIdentifier string is empty than I am trying to set all properties.
			if (propertyIdentifier == "") {

				targetObject = new();

				List<StringInputValidationError> majorNumberErrors = new();
				List<StringInputValidationError> minorNumberErrors = new();
				List<StringInputValidationError> patchNumberErrors = new();

				VersionNumberValueConverter(ref targetObject, "MajorNumber", inputStrings, out majorNumberErrors);
				VersionNumberValueConverter(ref targetObject, "MinorNumber", inputStrings, out minorNumberErrors);
				VersionNumberValueConverter(ref targetObject, "PatchNumber", inputStrings, out patchNumberErrors);

				errors = majorNumberErrors.Concat(minorNumberErrors.Concat(patchNumberErrors)).ToList();

				return;

			} else {

				int number = 0;
				errors = new();

				string invalidCharacteres = string.Concat(inputStrings[propertyIdentifier].Where(x => char.IsDigit(x) == false));

				if (invalidCharacteres.Length > 0) {
					errors.Add(new("Invalid Characters", propertyIdentifier)); // somehow need to make a tooltip too

				} else {

					try {
						number = int.Parse(inputStrings[propertyIdentifier]);

					} catch (Exception ex) {

						// It really should be an overflow exception but check anyway.
						if (ex is OverflowException) {
							errors.Add(new("Number Too Large", propertyIdentifier));

						} else {
							errors.Add(new("Unknown Error", propertyIdentifier));
						}
					}
					
					// This is clunky. Try to Find a better way.
					if (propertyIdentifier == "MajorNumber") {
						targetObject.MajorNumber = number;

					} else if (propertyIdentifier == "MinorNumber") {
						targetObject.MinorNumber = number;

					} else if (propertyIdentifier == "PatchNumber") {
						targetObject.PatchNumber = number;
					}

				}
			}
		}

	public string VersionDescription = "";
	public DateTime VersionReleaseDate;

	// Figure if/how to do version history later as it's not critical.
	//public List<VersionNumber, string, DateTime> VersionHistory;

	public string Name = "";
	public string Description = "";

	public SimpleStringInput<int> TestInt { get; private init; }

	public void TestIntValueConverter(ref int intValue, string inputString, out ReadOnlyCollection<StringInputValidationError> errors) {

			List<StringInputValidationError> newErrors = new();
			intValue = default;

			string invalidCharacteres = string.Concat(inputString.Where(x => char.IsDigit(x) == false));

			if (invalidCharacteres.Length > 0) {
				newErrors.Add(new("Invalid Characters"));

			} else {

				try {
					intValue = int.Parse(inputString);

				} catch (Exception ex) {

					// It really should be an overflow exception but check anyway.
					if (ex is OverflowException) {
						newErrors.Add(new("Number Too Large"));

					} else {
						newErrors.Add(new("Unknown Error"));
					}
				}

			}

			errors = newErrors.AsReadOnly();
		}

	public readonly StringInput<int> Year;

	public void YearValueConverter(ref int targetObject, string propertyIdentifier, Dictionary<string, string> inputStrings, out List<StringInputValidationError> errors) {

			errors = new();

			string invalidCharacteres = string.Concat(inputStrings[""].Where(x => char.IsDigit(x) == false));

			if (invalidCharacteres.Length > 0) {
				errors.Add(new("Invalid Characters", "")); // somehow need to make a tooltip too
				targetObject = 0; // Since it is a value type that is not nullable the best I can do is return the default value.

			} else {
				targetObject = int.Parse(inputStrings[""]);
			}

		}

}