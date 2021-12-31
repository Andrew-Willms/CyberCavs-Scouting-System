using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	public class GameEditingData {

		public GameEditingData() {

			Version = new(VersionNumberValueConverter, new string[] { "MajorNumber", "MinorNumber", "PatchNumber" }, new string[] { "0", "0", "0" });

			Year = new(YearValueConverter, new string[] { "" }, new string[] { "0" });

		}

		public readonly UserInput<VersionNumber> Version; // Cannot be initialized here because it needs to pass the instance member VersionNumberValueConverter

		public void VersionNumberValueConverter(ref VersionNumber targetObject, Dictionary<string, string> inputStrings,
			out List<UserInputValidationError> errors) {

			errors = new();
			Dictionary<string, int> versionComponents = new();

			foreach (KeyValuePair<string, string> input in inputStrings) {

				string invalidCharacteres = string.Concat(input.Value.Where(x => char.IsDigit(x) == false));

				if (invalidCharacteres.Length > 0) {
					errors.Add(new("Invalid Characters", UserInputValidationErrorSeverity.Error, input.Key)); // somehow need to make a tooltip too

				} else {
					versionComponents.Add(input.Key, int.Parse(input.Value));
				}
			}

			if (errors.Count == 0) {

				// Since there is no other state attached to VersionNumber and since VersionNumber is a small class it's easiest just to
				// instantiate a new object.
				targetObject = new(versionComponents["MajorNumber"], versionComponents["MinorNumber"], versionComponents["PatchNumber"]);

			} else {
				targetObject = null;
			}

			return;
		}

		public string VersionDescription = "";
		public DateTime VersionReleaseDate;

		// Figure if/how to do version history later as it's not critical.
		//public List<VersionNumber, string, DateTime> VersionHistory;

		public string Name = "";
		public string Description = "";

		public readonly UserInput<int> Year;

		public void YearValueConverter(ref int targetObject, Dictionary<string, string> inputStrings, out List<UserInputValidationError> errors) {

			errors = new();

			string invalidCharacteres = string.Concat(inputStrings[""].Where(x => char.IsDigit(x) == false));

			if (invalidCharacteres.Length > 0) {
				errors.Add(new("Invalid Characters", UserInputValidationErrorSeverity.Error, "")); // somehow need to make a tooltip too
				targetObject = 0; // Since it is a value type that is not nullable the best I can do is return the default value.

			} else {
				targetObject = int.Parse(inputStrings[""]);
			}

		}

	}

}