namespace CCSSDomain;

// Consider making this a struct
public class Version {

	public Version() { }

	public Version(uint major, uint minor, uint patch, string description = "") {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
		Description = description;
	}

	public uint MajorNumber { get; init; }

	public uint MinorNumber { get; init; }

	public uint PatchNumber { get; init; }

	public string Name { get; init; } = "";

	public string Description { get; init; } = "";

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}