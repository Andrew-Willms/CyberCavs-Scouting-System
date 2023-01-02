using System.Diagnostics.CodeAnalysis;

namespace CCSSDomain.GameSpecification;



// Consider making this a struct
public class Version {

	public Version() { }

	[SetsRequiredMembers]
	public Version(uint major, uint minor, uint patch, string name = "", string description = "") {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
		Name = name;
		Description = description;
	}

	public required uint MajorNumber { get; init; }

	public required uint MinorNumber { get; init; }

	public required uint PatchNumber { get; init; }

	public string Name { get; init; } = "";

	public string Description { get; init; } = "";

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}