using System;
using System.Diagnostics;
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
		uint allianceIndex,
		ReadOnlyList<uint> teams,
		ReadOnlyList<uint>? surrogates) {

		ReadOnlyList<uint> duplicates = teams.Duplicates().ToReadOnly();
		if (duplicates.Any()) {
			errorSink(DuplicateTeam.Create(duplicates) ?? throw new UnreachableException());
		}

		if (surrogates is not null) {
			foreach (uint surrogate in surrogates) {
				if (!teams.Contains(surrogate)) {
					errorSink(new SurrogateNotInMatch { Surrogate = surrogate });
				}
			}
		}

		return new(allianceIndex, teams, surrogates);
	}

}

public class DuplicateTeam : DomainError {

	public ReadOnlyList<uint> Duplicates { get; }

	private DuplicateTeam(ReadOnlyList<uint> duplicates) {

		Duplicates = duplicates;
	}

	public static DuplicateTeam? Create(ReadOnlyList<uint> duplicates) {

		return duplicates.Count == 0 ? null : new(duplicates);
	}

}

public class SurrogateNotInMatch : DomainError {

	public required uint Surrogate { get; init; }

}
