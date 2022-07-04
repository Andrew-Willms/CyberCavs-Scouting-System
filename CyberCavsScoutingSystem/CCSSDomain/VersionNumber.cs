using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSSDomain;

// Consider making this a struct
public class VersionNumber {

	public VersionNumber() { }

	public VersionNumber(uint major, uint minor, uint patch) {
		MajorNumber = major;
		MinorNumber = minor;
		PatchNumber = patch;
	}

	public uint MajorNumber { get; set; }

	public uint MinorNumber { get; set; }

	public uint PatchNumber { get; set; }

	public string VersionDescription { get; set; }

	public override string ToString() {
		return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
	}

}