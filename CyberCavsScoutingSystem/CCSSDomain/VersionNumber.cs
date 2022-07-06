namespace CCSSDomain;

// Consider making this a struct
public class VersionNumber {

	public VersionNumber() { }

	public VersionNumber(uint major, uint minor, uint patch, string description = "") {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
		Description = description;
	}

	public uint MajorNumber { get; }

	public uint MinorNumber { get; }

	public uint PatchNumber { get; }

	public string Description { get; init; } = "";

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}