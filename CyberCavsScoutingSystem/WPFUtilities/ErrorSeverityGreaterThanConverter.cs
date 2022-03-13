using System;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities;

public class ErrorSeverityGreaterThanConverter : IValueConverter {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		StringInputValidationErrorSeverity threshold, severity;

		if (value is StringInputValidationErrorSeverity valueAsSeverity) {
			severity = valueAsSeverity;
		} else {
			throw new ArgumentException($"The parameter \"{nameof(value)}\" cannot be converted to a StringInputValidationErrorSeverity");
		}

		if (parameter is StringInputValidationErrorSeverity parameterAsSeverity) {
			threshold = parameterAsSeverity;
		} else {
			throw new ArgumentException($"The parameter \"{nameof(parameter)}\" cannot be converted to a StringInputValidationErrorSeverity");
		}

		return severity > threshold;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("This conversion does not work in reverse.");
	}
}