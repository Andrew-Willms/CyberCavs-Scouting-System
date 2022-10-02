using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using CCSSDomain.Models;
using UtilitiesLibrary;

namespace GameMakerWpf.DomainData; 



public static class AllianceGenerator {

	private const int NamesTriedUntilDuplicateAllianceNameAccepted = 10;

	private static int RandomizedColorsUsed;

	private static readonly List<(string, Color)> RandomizedColors = ColorsHelper.DefaultColorsRandomized();

	public static Alliance GenerateAlliance(IEnumerable<string> allianceNames) {

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

		} while (allianceNamesHashed.Contains($"Alliance {colorNamePair.colorName}")) ;


		return new() {
			Name = $"{colorNamePair.colorName} Alliance",
			Color = colorNamePair.color
		};
	}

}