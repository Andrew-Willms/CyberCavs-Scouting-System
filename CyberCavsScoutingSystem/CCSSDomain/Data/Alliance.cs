using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UtilitiesLibrary.Collections;

namespace CCSSDomain.Data;



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
		Action<DomainError> errorSink,
		ErrorContext errorContext,
		uint allianceIndex,
		ReadOnlyList<uint> teams,
		ReadOnlyList<uint>? surrogates) {

		ReadOnlyList<uint> duplicates = teams.Duplicates().ToReadOnly();
		if (duplicates.Any()) {
			errorSink(DuplicateTeamInAlliance.Create(errorContext, duplicates) ?? throw new UnreachableException());
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

public class DuplicateTeamInAlliance : DomainError {

	public ReadOnlyList<uint> Duplicates { get; }

	[SetsRequiredMembers]
	private DuplicateTeamInAlliance(ErrorContext errorContext, ReadOnlyList<uint> duplicates) : base(errorContext) {

		Duplicates = duplicates;
	}

	public static DuplicateTeamInAlliance? Create(ErrorContext errorContext, ReadOnlyList<uint> duplicates) {

		if (duplicates.Count == 0) {
			return null;
		}

		return new(errorContext, duplicates);
	}

}

public class SurrogateNotPlayingInMatchError : DomainError {

	public uint Surrogate { get; }

	[SetsRequiredMembers]
	public SurrogateNotPlayingInMatchError(ErrorContext error, uint surrogate) : base(error) {

		Surrogate = surrogate;
	}

}
