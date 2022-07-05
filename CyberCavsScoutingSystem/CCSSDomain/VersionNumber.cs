namespace CCSSDomain;

// Consider making this a struct
public class VersionNumber {

	public VersionNumber() { }

	public VersionNumber(uint major, uint minor, uint patch) {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
	}

	public uint MajorNumber { get; }

	public uint MinorNumber { get; }

	public uint PatchNumber { get; }

	public string VersionDescription { get; init; } = "";

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}