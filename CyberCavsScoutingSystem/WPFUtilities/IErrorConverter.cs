﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using UtilitiesLibrary.Validation.Errors;

namespace WPFUtilities;



public interface IErrorConverter<TSeverityEnum, TConversionType> : IValueConverter
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum> {

	// This can't be static abstract because static abstract members can only be accessed from implementation
	// of the interface, not the interface it's self.
	public ReadOnlyDictionary<TSeverityEnum, TConversionType> ConversionDictionary { get; }

	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		TSeverityEnum severity = value as TSeverityEnum ?? throw new ArgumentException($"The object to be converted is not a {typeof(TSeverityEnum)}");

		return ConversionDictionary[severity] ?? throw new NullReferenceException();
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException($"IErrorConverter cannot be used to convert from another object back to an {typeof(TSeverityEnum)}");
	}

}