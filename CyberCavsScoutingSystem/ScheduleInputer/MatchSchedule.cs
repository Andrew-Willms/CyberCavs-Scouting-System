using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleInputer; 



public class MatchSchedule {

	//private List<Match> Matches = new();

	private bool CheckMatches(List<Match> matches) {

		Dictionary<int, int> occurrences = new();

		foreach (Match match in matches) {

			if (occurrences.ContainsKey(match.Red1)) {
				occurrences[match.Red1] += 1;
			} else {
				occurrences.Add(match.Red1, 1);
			}
		}

		int numberOfOccurrences = occurrences.First().Value;

		return occurrences.All(keyValuePair => keyValuePair.Value == numberOfOccurrences);
	}

}

public class Match {

	public DateTime Time { get; set; }

	public int Red1 { get; set; }
	public int Red2 { get; set; }
	public int Red3 { get; set; }

	public int Blue1 { get; set; }
	public int Blue2 { get; set; }
	public int Blue3 { get; set; }

}