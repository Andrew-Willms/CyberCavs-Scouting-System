using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesLibrary.Optional;

namespace UtilitiesLibrary.Collections;



public static class CollectionExtensions {

	public static List<T> Listify<T>(this T item) {

		return new() { item };
	}

	public static ReadOnlyList<T> ToReadOnly<T>(this IEnumerable<T> enumerable) {

		return new(enumerable);
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

	public static IEnumerable<TTarget?> SelectIfNotNull<TCollection, TTarget>
		(this IEnumerable<TCollection> enumerable, Func<TCollection, TTarget?> selector) {

		return enumerable.Select(selector).Where(x => x is not null);
	}

	public static IEnumerable<TTarget> SelectIfHasValue<TCollection, TTarget>
		(this IEnumerable<TCollection> enumerable, Func<TCollection, Optional<TTarget>> selector) {

		return enumerable.Where(x => selector(x).HasValue).Select(x => selector(x).Value);

		//return enumerable.Select(selector).Where(x => x.HasValue).Select(x => x.Value);
	}



	public static ReadOnlyList<T> ReadOnlyListify<T>(this T item) {

		return new List<T> { item }.ToReadOnly();
	}



	public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> predicate) {

		foreach (T item in enumerable) {
			predicate(item);
		}
	}



	public static bool IsEmpty<T>(this IEnumerable<T> enumerable) {

		return !enumerable.Any();
	}

	public static bool None<T>(this IEnumerable<T> enumerable, T value) where T : IComparable {

		return enumerable.Count(x => x.CompareTo(value) == 0) == 0;
	}

	public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) {

		return !enumerable.Any(predicate);
	}

	public static bool OnlyOne<T>(this IEnumerable<T> enumerable) {

		return enumerable.Count() == 1;
	}

	public static bool OnlyOne<T>(this IEnumerable<T> enumerable, T value) where T : IComparable {

		return enumerable.Count(x => x.CompareTo(value) == 0) == 1;
	}

	public static bool OnlyOne<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) {

		return enumerable.Count(predicate) == 1;
	}

	public static bool Multiple<T>(this IEnumerable<T> enumerable) {

		return enumerable.Count() > 1;
	}

	public static bool Multiple<T>(this IEnumerable<T> enumerable, T value) where T : IComparable {

		return enumerable.Count(x => x.CompareTo(value) == 0) > 1;
	}

	public static bool Multiple<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) {

		return enumerable.Count(predicate) > 1;
	}



	public static string CharArrayToString(this IEnumerable<char> enumerable) {

		return enumerable.Aggregate("", (current, character) => current + character);
	}



	public static ReadOnlyKeysDictionary<TKey, TValue> ToReadOnlyKeysDictionary<TKey, TValue>(
		this IEnumerable<TKey> keys, TValue defaultValue) where TKey : notnull {

		return new ReadOnlyKeysDictionary<TKey, TValue>(keys, defaultValue);
	}

}