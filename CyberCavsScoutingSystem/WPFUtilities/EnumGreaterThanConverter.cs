using System;
using System.Globalization;
using System.Windows.Data;
using UtilitiesLibrary.SmartEnum;

namespace WPFUtilities;



public class EnumGreaterThanConverter<TEnum> : IValueConverter where TEnum : OrderedSmartEnum<TEnum> {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		TEnum severity = value as TEnum ?? throw new ArgumentException(
			$"The parameter \"{nameof(value)}\" cannot be converted to a StringInputValidationErrorSeverity");

		TEnum threshold = parameter as TEnum ?? throw new ArgumentException(
			$"The parameter \"{nameof(parameter)}\" cannot be converted to a StringInputValidationErrorSeverity");

		return severity.CompareTo(threshold) > 0;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException("This conversion does not work in reverse.");
	}

}