using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPFUtilities.Extensions;



public static class CollectionExtensions {

	public static ReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> enumerable) {

		return new(enumerable.ToList());
	}

	public static void AddIfNotNull<T>(this List<T> enumerable, T? newValue) {

		if (newValue is null) {
			return;
		}

		enumerable.Add(newValue);
	}

	public static void AddIfHasValue<T>(this List<T> enumerable, Optional<T> newValue) {

		if (newValue.HasValue) {
			enumerable.Add(newValue.Value);
		}
	}

	public static IEnumerable<T> AppendIfNotNull<T>(this IEnumerable<T> enumerable, T? newValue) {

		return newValue is null ? enumerable : enumerable.Append(newValue);
	}

	public static ReadOnlyList<T> ReadOnlyListify<T>(this T item) {

		return new List<T> { item }.ToReadOnly();
	}

	public static bool OnlyOne<T>(this IEnumerable<T> enumerable, T value) where T : IComparable {

		return enumerable.Count(x => x.CompareTo(value) == 0) == 1;
	}

	public static bool Multiple<T>(this IEnumerable<T> enumerable, T value) where T : IComparable {

		return enumerable.Count(x => x.CompareTo(value) == 0) > 1;
	}

}