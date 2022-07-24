using System;
using System.IO;

namespace CCSSDomain.Data; 



public static class GameNameGenerator {

	public static string GetRandomGameName() {

		return $"{GetRandomAdjective()} {GetRandomNoun()}";
	}

	private static string GetRandomAdjective() {

		string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
		string fileDirectory = solutionDirectory + "\\\\CCSSDomain\\\\Data\\\\" + "adjectives.txt";

		string[] lines = File.ReadAllLines(fileDirectory);
		Random rand = new();
		return lines[rand.Next(lines.Length)];
	}

	private static string GetRandomNoun() {

		string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
		string fileDirectory = solutionDirectory + "\\\\CCSSDomain\\\\Data\\\\" + "nouns.txt";

		string[] lines = File.ReadAllLines(fileDirectory);
		Random rand = new();
		return lines[rand.Next(1, lines.Length)].Split(',')[0];
	}

}