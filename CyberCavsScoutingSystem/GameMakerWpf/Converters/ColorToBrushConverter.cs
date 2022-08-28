using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using UtilitiesLibrary;

namespace GameMakerWpf.Converters; 

public class ColorToBrushConverter  : IValueConverter {
	
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		if (value is not Optional<Color> colorOption) {
			throw new ArgumentException($"The parameter {nameof(value)} could not be converted to a {typeof(Color)}.");
		}

		return colorOption.HasValue
			? new SolidColorBrush(colorOption.Value)
			: new SolidColorBrush(Colors.Black);
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		
		if (value is not SolidColorBrush brush) {
			throw new ArgumentException($"The parameter {nameof(value)} could not be converted to a {typeof(Brush)}.");
		}

		return brush.Color;
	}

}