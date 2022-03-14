﻿using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;

namespace WPFUtilities;

public interface IErrorConverter<TSeverityEnum, TConversionType> : IValueConverter where TSeverityEnum : Enum  {

	public ReadOnlyDictionary<TSeverityEnum, TConversionType> ConversionDictionary { get; }

	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		TSeverityEnum severity;

		if (value is TSeverityEnum valueASeverity) {
			severity = valueASeverity;
		} else {
			throw new ArgumentException($"The object to be converted is not a {typeof(TSeverityEnum)}");
		}

		TConversionType conversionResult = ConversionDictionary[severity];

		if (conversionResult is null) {
			throw new NullReferenceException();
		}

		return conversionResult;
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException($"IErrorConverter cannot be used to convert from another object back to an {typeof(TSeverityEnum)}");
	}
}