using System.Collections.Generic;
using System.Linq;

namespace UtilitiesLibrary.Validation.Errors;



public static class ErrorsExtensions {

	public static bool AreFatal<T>(this IEnumerable<ValidationError<T>> errors)
		where T : ValidationErrorSeverityEnum<T>, IValidationErrorSeverityEnum<T> {

		return errors.Any(x => x.Severity.IsFatal);
	}

}