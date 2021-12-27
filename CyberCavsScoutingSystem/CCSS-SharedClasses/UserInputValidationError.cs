using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	// I think these things need to be static
	public record UserInputValidationError {

		public string Name { get; init; }

		public object Tooltip { get; init; }

		public UserInputValidationErrorSeverity Severity { get; set; }

		//public sometingHere ToolTipStyle { get; init; }
	}

	public enum UserInputValidationErrorSeverity {
		Note = 0,
		Advisory = 1,
		Warning = 2,
		Error = 3
	}
}
