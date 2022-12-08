using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UtilitiesLibrary.Collections; 



// TODO find a better or more specific name for this. Possibly involving "IndirectAdd"
public class ObservableList<TItem, TAdd> : INotifyCollectionChanged, IEnumerable<TItem> {

	private readonly List<TItem> Collection = new();
	public TItem this[int index] => Collection[index];

	public Action<TItem>? OnAdd { private get; init; }
	public Action<TItem>? OnRemove { private get; init; }

	public required Func<TAdd, TItem> Adder { private get; init; }



	public void Add(TAdd intermediateItem) {

		TItem newItem = Adder.Invoke(intermediateItem);

		Collection.Add(newItem);
		CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Add, newItem));

		OnAdd?.Invoke(newItem);
	}

	public Result<ListRemoveError> Remove(TItem toRemove) {

		if (!Collection.Contains(toRemove)) {
			return ListRemoveError.NotFound;
		}

		int index = Collection.IndexOf(toRemove);
		if (!Collection.Remove(toRemove)) {
			return ListRemoveError.OtherFailure;
		}

		OnRemove?.Invoke(toRemove);
		CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, toRemove, index));
		return new Success();
	}



	public IEnumerator<TItem> GetEnumerator() {
		return Collection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}



	public event NotifyCollectionChangedEventHandler? CollectionChanged;

}



// TODO: Move to it's own class
public class ListRemoveError : Error<ListRemoveError.Types> {

	// I think the only reasons List.Remove can fail is if the item is not found or the list is
	// being enumerated in a foreach loop but I don't want to assume that.
	public enum Types {
		ItemNotFound,
		OtherFailure
	}

	public static ListRemoveError NotFound => new() { ErrorType = Types.ItemNotFound };

	public static ListRemoveError OtherFailure => new() { ErrorType = Types.OtherFailure };

}