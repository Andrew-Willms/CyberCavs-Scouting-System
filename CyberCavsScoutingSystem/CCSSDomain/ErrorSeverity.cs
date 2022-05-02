using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities;

namespace CCSSDomain;



public class ErrorSeverity : ValidationErrorSeverityEnum<ErrorSeverity>, IValidationErrorSeverityEnum<ErrorSeverity> {

	public static readonly ErrorSeverity None = new (nameof(None), 0, false);
	public static readonly ErrorSeverity Info = new (nameof(Info), 1, false);
	public static readonly ErrorSeverity Advisory = new (nameof(Advisory), 2, false);
	public static readonly ErrorSeverity Warning = new (nameof(Warning), 3, false);
	public static readonly ErrorSeverity Error = new (nameof(Error), 4, true);

	public static ErrorSeverity NoError => None;

	private ErrorSeverity(string name, int value, bool isFatal) : base(name, value, isFatal) { }
}