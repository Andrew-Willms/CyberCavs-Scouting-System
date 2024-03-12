using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DataIngester.Converters; 



public class BoolToAccessibilityIndicatorColorConverter : IValueConverter {

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		if (value is not bool isAccessible) {
			throw new ArgumentException();
		}

		if (targetType != typeof(Color)) {
			throw new ArgumentException();
		}

		return isAccessible ? Colors.Green : Colors.Red;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new InvalidOperationException("This converter cannot be used in the backwards direction.");
	}

}