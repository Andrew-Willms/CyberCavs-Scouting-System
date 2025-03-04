using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ScoutingApp.Views.Converters;



public abstract class NotNullToBoolConverter<T> : IValueConverter {

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {

		if (targetType != typeof(bool)) {
			throw new NotSupportedException();
		}
		
		return value is not null;
	}
	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
		throw new NotSupportedException();
	}

}

public class StringNotNullToBoolConverter : NotNullToBoolConverter<string> {



}