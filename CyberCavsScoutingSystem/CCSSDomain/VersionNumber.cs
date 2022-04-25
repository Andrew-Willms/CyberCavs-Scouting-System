using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSSDomain;

// Consider making this a struct
public class VersionNumber {

	public VersionNumber() { }

	public VersionNumber(int major, int minor, int patch) {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
	}

	public int MajorNumber { get; set; }

	public int MinorNumber { get; set; }

	public int PatchNumber { get; set; }

	public int VersionDescription { get; set; }

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}