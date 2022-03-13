using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities;

public interface IErrorConverter<T> : IValueConverter {

	public ReadOnlyDictionary<StringInputValidationErrorSeverity, T> ConversionDictionary { get; }
	
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		StringInputValidationErrorSeverity severity;

		if (value is StringInputValidationErrorSeverity valueASeverity) {
			severity = valueASeverity;
		} else {
			throw new ArgumentException($"The object to be converted is not a {typeof(StringInputValidationErrorSeverity)}");
		}

		T conversionResult = ConversionDictionary[severity];

		if (conversionResult == null) {
			throw new NullReferenceException();
		}

		return conversionResult;
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException($"IErrorConverter cannot be used to convert from another object back to an {typeof(StringInputValidationErrorSeverity)}");
	}
}