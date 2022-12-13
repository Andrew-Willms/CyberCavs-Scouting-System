using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UtilitiesLibrary.Results;

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

	public IListRemoveResult<TItem> Remove(TItem toRemove) {

		if (!Collection.Contains(toRemove)) {
			return new IListRemoveResult<TItem>.ItemNotFound();
		}

		int index = Collection.IndexOf(toRemove);
		if (!Collection.Remove(toRemove)) {
			return new IListRemoveResult<TItem>.OtherFailure();
		}

		OnRemove?.Invoke(toRemove);
		CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Remove, toRemove, index));
		return new IListRemoveResult<TItem>.Success();
	}



	public IEnumerator<TItem> GetEnumerator() {
		return Collection.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}



	public event NotifyCollectionChangedEventHandler? CollectionChanged;

}



public interface IListRemoveResult : IResult { }

// TODO: Move to it's own file
public interface IListRemoveResult<T> : IListRemoveResult {

	public class Success : IResult.Success, IListRemoveResult<T> { }

	public class ItemNotFound : Error, IListRemoveResult<T> { }

	public class OtherFailure : Error, IListRemoveResult<T> { }

}