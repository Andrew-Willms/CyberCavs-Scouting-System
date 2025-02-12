using System;
using System.Collections.Generic;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Results;

namespace CCSSDomain.MatchData;



public class Alliance {

	public uint AllianceIndex { get; }

	public ReadOnlyList<uint> Teams { get; }

	public ReadOnlyList<uint>? Surrogates { get; }

	private Alliance(out object errorSink, uint allianceIndex, ReadOnlyList<uint> teams, ReadOnlyList<uint>? surrogates) {




		AllianceIndex = allianceIndex;
		Teams = teams;
		Surrogates = surrogates;
	}

	public static IResult<Alliance> Create(GameSpec gameSpec, Action<DomainError> errorSink, uint allianceIndex, ReadOnlyList<uint> teams, ReadOnlyList<uint>? surrogates) {

		if (allianceIndex >= gameSpec.AlliancesPerMatch) {
			return new IResult<Alliance>.Error($"An AllianceIndex of {allianceIndex} was specified. The maximum allowed AllianceIndex is {gameSpec.AlliancesPerMatch - 1}.");
		}

		List<uint> duplicates = teams.Duplicates();
		if (duplicates.Count == 1) {
			return new IResult<Alliance>.Error($"Duplicates of team {duplicates.First()} was specified.");
		}

		if (duplicates.Count > 1) {
			return new IResult<Alliance>.Error($"Duplicates of teams {duplicates.StringJoinDifferentLast(", ", ", and ")} were specified.");
		}

		if (surrogates is not null) {
			foreach (uint surrogate in surrogates) {
				if (!teams.Contains(surrogate)) {
					return new IResult<Alliance>.Error($"Surrogate team {surrogate} is not a team playing in this match.");
				}
			}
		}



		return new IResult<Alliance>.Success(new(allianceIndex, teams, surrogates));
	}

}