using System;
using System.Windows.Data;
using System.Globalization;
using WPFUtilities;
using CCSSDomain;

namespace GameMaker.Converters;

//public class EnumGreaterThanConverter<TEnum> : IValueConverter where TEnum : Enum {

//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

//		TEnum threshold, severity;

//		if (value is TEnum valueAsTEnum) {
//			severity = valueAsTEnum;
//		} else {
//			throw new ArgumentException($"The parameter \"{nameof(value)}\" cannot be converted to a StringInputValidationErrorSeverity");
//		}

//		if (parameter is TEnum parameterAsTEnum) {
//			threshold = parameterAsTEnum;
//		} else {
//			throw new ArgumentException($"The parameter \"{nameof(parameter)}\" cannot be converted to a StringInputValidationErrorSeverity");
//		}

//		return (severity.CompareTo(threshold) > 0);
//	}

//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
//		throw new NotImplementedException("This conversion does not work in reverse.");
//	}
//}

public class ErrorGreaterThanConverter : WPFUtilities.EnumGreaterThanConverter<CCSSDomain.ErrorSeverity> { }