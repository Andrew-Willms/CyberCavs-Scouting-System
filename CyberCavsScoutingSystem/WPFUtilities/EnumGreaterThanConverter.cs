using System;
using System.Globalization;
using System.Windows.Data;
using UtilitiesLibrary.SmartEnum;

namespace WPFUtilities;



public class EnumGreaterThanConverter<TEnum> : IValueConverter where TEnum : OrderedSmartEnum<TEnum> {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		TEnum threshold, severity;

		if (value is TEnum valueAsTEnum) {
			severity = valueAsTEnum;
		} else {
			throw new ArgumentException($"The parameter \"{nameof(value)}\" cannot be converted to a StringInputValidationErrorSeverity");
		}

		if (parameter is TEnum parameterAsTEnum) {
			threshold = parameterAsTEnum;
		} else {
			throw new ArgumentException($"The parameter \"{nameof(parameter)}\" cannot be converted to a StringInputValidationErrorSeverity");
		}

		return severity.CompareTo(threshold) > 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("This conversion does not work in reverse.");
	}
}