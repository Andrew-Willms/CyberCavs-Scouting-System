using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.MatchData;



public class Alliance {

	public uint AllianceIndex { get; }

	public ReadOnlyList<uint> Teams { get; }

	public ReadOnlyList<uint>? Surrogates { get; }

	private Alliance(uint allianceIndex, ReadOnlyList<uint> teams, ReadOnlyList<uint>? surrogates) {

		AllianceIndex = allianceIndex;
		Teams = teams;
		Surrogates = surrogates;
	}

	public static Alliance Create(
		GameSpec gameSpec,
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		uint allianceIndex,
		ReadOnlyList<uint> teams,
		ReadOnlyList<uint>? surrogates) {

		if (allianceIndex >= gameSpec.AlliancesPerMatch) {
			errorSink(new AllianceIndexOutOfRangeError(errorContext, allianceIndex, gameSpec.AlliancesPerMatch - 1));
		}

		ReadOnlyList<uint> duplicates = teams.Duplicates().ToReadOnly();
		if (duplicates.Any()) {
			errorSink(new DuplicateTeamInAllianceError(errorContext, duplicates));
		}

		if (surrogates is not null) {
			foreach (uint surrogate in surrogates) {
				if (!teams.Contains(surrogate)) {
					errorSink(new SurrogateNotPlayingInMatchError(errorContext, surrogate));
				}
			}
		}

		return new(allianceIndex, teams, surrogates);
	}

}

public class AllianceIndexOutOfRangeError : DomainError {

	public uint AllianceIndex { get; }

	public uint MaxAllianceIndex { get; }

	public string Message { get; }

	[SetsRequiredMembers]
	public AllianceIndexOutOfRangeError(ErrorContext error, uint allianceIndex, uint maxAllianceIndex) : base(error) {

		AllianceIndex = allianceIndex;
		MaxAllianceIndex = maxAllianceIndex;
		Message = $"An AllianceIndex of {allianceIndex} was specified. The maximum allowed AllianceIndex is {maxAllianceIndex}.";
	}

}

public class DuplicateTeamInAllianceError : DomainError {

	public ReadOnlyList<uint> Duplicates { get; }

	public string Message { get; }

	[SetsRequiredMembers]
	public DuplicateTeamInAllianceError(ErrorContext error, ReadOnlyList<uint> duplicates) : base(error) {

		Duplicates = duplicates;

		Message = duplicates.Count switch {
			0 => "Duplicate Team in Alliance error created but no duplicates. This should not happen.",
			1 => $"Duplicates of team {duplicates.First()} was specified.",
			> 1 => $"Duplicates of teams {duplicates.StringJoinCustom(", ", ", and ", " and ")} were specified.",
			_ => throw new UnreachableException()
		};
	}

}

public class SurrogateNotPlayingInMatchError : DomainError {

	public uint Surrogate { get; }

	public string Message { get; }

	[SetsRequiredMembers]
	public SurrogateNotPlayingInMatchError(ErrorContext error, uint surrogate) : base(error) {

		Surrogate = surrogate;
		Message = $"Surrogate team {surrogate} is not a team playing in this match.";
	}

}
