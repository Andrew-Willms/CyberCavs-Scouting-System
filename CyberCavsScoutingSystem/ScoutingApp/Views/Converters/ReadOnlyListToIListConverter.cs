using Microsoft.Maui.Controls;
using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CCSSDomain.GameSpecification;
using UtilitiesLibrary.Collections;

namespace ScoutingApp.Views.Converters;



public abstract class ReadOnlyListToIListConverter<T> : IValueConverter {

	protected abstract Type NonGenericReadOnlyListType { get; }
	protected abstract Type NonGenericIListType { get; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

		ReadOnlyList<T> readOnlyList =
			value as ReadOnlyList<T>
			?? throw new ArgumentException($"The parameter \"{nameof(value)}\" is of type \"{value.GetType()}\" " +
										   $"which is not assignable to the type \"{NonGenericReadOnlyListType}\".");

		if (!typeof(IList).IsAssignableTo(targetType)) {
			throw new ArgumentException($"The value of \"{nameof(targetType)}\" is \"{targetType}\" but this converter " +
										$"converts to the type \"{NonGenericIListType}\".");
		}

		T[] array = new T[readOnlyList.Count];
		readOnlyList.CopyTo(array, 0);

		return array.AsReadOnly();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {

		//TODO not sure if this actually works
		IList<T> iList =
			value as IList<T>
			?? throw new ArgumentException($"The parameter \"{nameof(value)}\" is of type \"{value.GetType()}\" " +
										   $"which is not assignable to the type \"{NonGenericIListType}\".");

		if (!targetType.IsAssignableTo(NonGenericReadOnlyListType)) {
			throw new ArgumentException($"The value of \"{nameof(targetType)}\" is \"{targetType}\" but this converter " +
										$"converts to the type \"{NonGenericIListType}\".");
		}

		return new ReadOnlyList<T>(iList.ToArray());
	}

}

public class ReadOnlyListOfStringsToIListConverter : ReadOnlyListToIListConverter<string> {

	protected override Type NonGenericReadOnlyListType { get; } = typeof(ReadOnlyList<string>);

	protected override Type NonGenericIListType { get; } = typeof(IList<string>);

}

public class ReadOnlyListOfAlliancesToIListConverter : ReadOnlyListToIListConverter<AllianceColor> {

	protected override Type NonGenericReadOnlyListType { get; } = typeof(ReadOnlyList<AllianceColor>);

	protected override Type NonGenericIListType { get; } = typeof(IList<AllianceColor>);

}