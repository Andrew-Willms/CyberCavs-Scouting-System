using System.Collections.Generic;
using System.Linq;

namespace WPFUtilities; 



public static class CollectionExtensions {

	public static ReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> enumerable) {

		return (ReadOnlyList<T>)enumerable.ToList().AsReadOnly();
	}

	public static void AddIfNotNull<T>(this List<T> enumerable, T? newValue) {

		if (newValue is null) {
			return;
		}

		enumerable.Add(newValue);
	}

	public static IEnumerable<T> AppendIfNotNull<T>(this IEnumerable<T> enumerable, T? newValue) {

		return newValue is null ? enumerable : enumerable.Append(newValue);
	}
}