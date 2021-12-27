using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	public class GameEditingData {

		public VersionNumber Version = new(0, 0, 0);
		public string VersionDescription = "";
		public DateTime VersionReleaseDate;

		// Figure if/how to do version history later as it's not critical.
		//public List<VersionNumber, string, DateTime> VersionHistory;

		public string Name = "";
		public string Description = "";

		private readonly UserInput<int> Year = "0";
		//public UserInputBindingData<int> Year { get => _Year; }



	}

}