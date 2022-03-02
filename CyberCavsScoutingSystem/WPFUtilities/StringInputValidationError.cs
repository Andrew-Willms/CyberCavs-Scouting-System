using System;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFUtilities;

// I think these things need to be static
public record StringInputValidationError {

	public string Name { get; private init; }

	public StringInputValidationErrorSeverity Severity { get; private init; }

	// This can be used to provide additional identifying information about the error.
	// For example, if multi data binding is used to bind multiple View elements to a single UserMultiInput class
	// the Identifier parameter can be used to indicate which of the multiple view elements the error relates to.
	public string Identifier { get; private init; }

	public object Tooltip { get; init; }

	//public somethingHere ToolTipStyle { get; init; }

	public StringInputValidationError(string name, string identifier = "", StringInputValidationErrorSeverity severity = StringInputValidationErrorSeverity.Error) {
		Name = name;
		Severity = severity;
		Identifier = identifier;
	}

}

// These are just placeholder for now. Change as needed.
public enum StringInputValidationErrorSeverity {
	None,
	Note,
	Advisory,
	Warning,
	Error
}

public enum ErrorColorType {
	BorderBrush,
	Foreground,

}

public static class ErrorSeverityConverter {

	public static ReadOnlyDictionary<StringInputValidationErrorSeverity, Color> ErrorColors { get; } = new(
		new Dictionary<StringInputValidationErrorSeverity, Color>() {
			{ StringInputValidationErrorSeverity.Note, Colors.Gray },
			{ StringInputValidationErrorSeverity.Advisory, Colors.Yellow },
			{ StringInputValidationErrorSeverity.Warning, Colors.Orange },
			{ StringInputValidationErrorSeverity.Error, Colors.Red }
		});

	public static Color ToColor(StringInputValidationErrorSeverity severity) {
		Trace.WriteLine(severity);
		Trace.WriteLine(ErrorColors[severity]);
		Trace.WriteLine(Colors.Red);
		//return ErrorColors[severity];
		return Colors.Yellow;
	}

	public static Brush ToBrush(StringInputValidationErrorSeverity severity) {
		return new SolidColorBrush(ErrorColors[severity]);
	}
}

public class ErrorSeverityToBrushConverter : IValueConverter {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		if (value is StringInputValidationErrorSeverity severity) {
			return ErrorSeverityConverter.ToBrush(severity);
		}

		throw new ArgumentException("The object to be converted is not a StringInputValidationErrorSeverity");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("You should not be getting the error severity by color.");
	}
}

public class ErrorSeverityToColorConverter : IValueConverter {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		if (value is StringInputValidationErrorSeverity severity) {
			return ErrorSeverityConverter.ToColor(severity);
		}

		throw new ArgumentException("The object to be converted is not a StringInputValidationErrorSeverity");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("You should not be getting the error severity by color.");
	}
}

public class ErrorSeverityGreaterThanConverter : IValueConverter {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		StringInputValidationErrorSeverity threshold, severity;

		//if (parameter is StringInputValidationErrorSeverity parameterErrorSeverity) {
		//	threshold = parameterErrorSeverity;
		//} else {
		//	throw new ArgumentException($"The parameter \"{nameof(parameter)}\" cannot be converted to StringInputValidationErrorSeverity");
		//}

		if (value is StringInputValidationErrorSeverity valueErrorSeverity) {
			severity = valueErrorSeverity;
		} else {
			throw new ArgumentException($"The parameter \"{nameof(value)}\" cannot be converted to a StringInputValidationErrorSeverity");
		}

		//return severity > threshold;
		return severity > StringInputValidationErrorSeverity.None;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("This conversion does not work in reverse.");
	}
}