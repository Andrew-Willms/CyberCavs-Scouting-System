using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	// With .net 6.0 and c# 10 I could make this a "readonly record struct" and use the built in positional constructor.
	// Unfortunately I would have to upgrade to Visual Studio 2022 to use .net 6.0 and I don't want to deal with that right now.
	public readonly struct VersionNumber {

		// I am not sure if I need a constructor in this case.
		// I have included one since creating the object using a constructor is more concise than with the initializers.
		public VersionNumber(int major, int minor, int patch) {
			MajorNumber = major;
			MinorNumber = minor;
			PatchNumber = patch;
		}

		public int MajorNumber { get; init; }

		public int MinorNumber { get; init; }

		public int PatchNumber { get; init; }

		public override string ToString() {
			return $"{MajorNumber}.{MinorNumber}.{PatchNumber}";
		}

	}

}
