using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using GameMakerWpf.Domain.EditingData;
using UtilitiesLibrary;

namespace GameMakerWpf.Domain.Data;



public static class AllianceGenerator {

	private const int NamesTriedUntilDuplicateAllianceNameAccepted = 10;

	private static int RandomizedColorsUsed;

	private static readonly List<(string, Color)> RandomizedColors = ColorsHelper.DefaultColorsRandomized();

	public static AllianceEditingData GenerateUniqueAlliance(IEnumerable<string> allianceNames) {

		HashSet<string> allianceNamesHashed = allianceNames.ToHashSet();

		int namesTried = 0;
		(string colorName, Color color) colorNamePair;

		do {

			colorNamePair = RandomizedColors[RandomizedColorsUsed];
			RandomizedColorsUsed++;
			namesTried++;

			if (namesTried == NamesTriedUntilDuplicateAllianceNameAccepted) {
				break;
			}

			if (RandomizedColorsUsed == RandomizedColors.Count) {
				RandomizedColorsUsed = 0;
			}

		} while (allianceNamesHashed.Contains($"Alliance {colorNamePair.colorName}"));


		return new() {
			Name = $"{colorNamePair.colorName} Alliance",
			RedColorValue = colorNamePair.color.R.ToString(),
			GreenColorValue = colorNamePair.color.G.ToString(),
			BlueColorValue = colorNamePair.color.B.ToString(),

		};
	}

}