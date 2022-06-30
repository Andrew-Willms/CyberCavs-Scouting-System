using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPFUtilities; 



public class ReadOnlyList<T> : ReadOnlyCollection<T>, IEnumerable<T> {

	public ReadOnlyList() : base(new List<T>()) { }

	public ReadOnlyList(IList<T> list) : base(list) { }

	public ReadOnlyList(params T[] items) : base(items.ToList()) { }

	public ReadOnlyList<T> CopyAndAdd(T newItem) {

		T[] array = new T[Count + 1];

		CopyTo(array, 0);

		array[Count] = newItem;

		return array.ToList().ToReadOnly();
	}

	public ReadOnlyList<T> CopyAndAddRange(IEnumerable<T> newItems) {

		IEnumerable<T> enumerable = newItems.ToList();

		T[] array = new T[Count + enumerable.Count()];

		CopyTo(array, 0);

		int currentIndex = Count;
		foreach (T item in enumerable) {
			array[currentIndex] = item;
			currentIndex++;
		}

		return array.ToList().ToReadOnly();
	}

	public ReadOnlyList<T> CopyAndAddRange(params T[] newItems) {

		return CopyAndAddRange(newItems.AsEnumerable());
	}

	public ReadOnlyList<T> CopyAndAddRanges(params IEnumerable<T>[] newItems) {

		return CopyAndAddRange(newItems.SelectMany(x => x)); // I think this should work
	}

	public static readonly ReadOnlyList<T> Empty = new();
}