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

		//public sometingHere ToolTipStyle { get; init; }
	}

}
