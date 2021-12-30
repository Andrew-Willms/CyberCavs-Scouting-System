﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCSS_SharedClasses {

	// I think these things need to be static
	public record UserInputValidationError {

		public string Name { get; init; }

		// This can be used to provide additional identifying information about the error.
		// For example, if multi data binding is used to bind multiple View elements to a single UserMultiInput class
		// the Identifier parameter can be used to indicate which of the multiple view elements the error relates to.
		public string Identifier { get; init; }

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
