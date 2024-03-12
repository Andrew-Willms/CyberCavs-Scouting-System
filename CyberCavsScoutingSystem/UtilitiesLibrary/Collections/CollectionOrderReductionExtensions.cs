using System.Collections.Generic;
using System.Linq;

namespace UtilitiesLibrary.Collections;



public static class CollectionOrderReductionExtensions {

	public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> twoDimensionalEnumerable) {

		List<T> flattenedList = new();

		twoDimensionalEnumerable.Foreach(flattenedList.AddRange);

		return flattenedList.ToReadOnly();
	}


	public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enumerable, IEnumerable<T> newItems) {

		return newItems.Aggregate(enumerable, (current, item) => current.Append(item));
	}

	public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> enumerable, T[] newItems) {

		return newItems.Aggregate(enumerable, (current, item) => current.Append(item));
	}



	public static IEnumerable<T> AppendRanges<T>(this IEnumerable<T> enumerable,
		params IEnumerable<T>[] newEnumerables) {

		return newEnumerables.Aggregate(enumerable, (current, newEnumerable) => current.AppendRange(newEnumerable));
	}

	public static IEnumerable<T> AppendRanges<T>(this IEnumerable<T> enumerable,
		IEnumerable<IEnumerable<T>> newEnumerables) {

		return newEnumerables.Aggregate(enumerable, (current, newEnumerable) => current.AppendRange(newEnumerable));
	}

}