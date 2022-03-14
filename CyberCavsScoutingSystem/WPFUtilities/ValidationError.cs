using System;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities;

// I think these things need to be static
public record ValidationError<TSeverityEnum> where TSeverityEnum : Enum {

	public string Name { get; private init; }

	public TSeverityEnum Severity { get; private init; }

	// This can be used to provide additional identifying information about the error.
	// For example, if multi data binding is used to bind multiple View elements to a single UserMultiInput class
	// the Identifier parameter can be used to indicate which of the multiple view elements the error relates to.
	public string Identifier { get; private init; }

	public ValidationError(string name, TSeverityEnum severity, string identifier = "") {
		Name = name;
		Severity = severity;
		Identifier = identifier;
	}

}

//// These are just placeholder for now. Change as needed.
//public enum StringInputValidationErrorSeverity {
//	None,
//	Note,
//	Advisory,
//	Warning,
//	Error
//}

//public enum ErrorColorType {
//	BorderBrush,
//	Foreground,
//}