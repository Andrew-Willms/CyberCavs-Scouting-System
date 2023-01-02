using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UtilitiesLibrary.Collections;



public class ReadOnlyList {

    public static readonly ReadOnlyList Empty = new();

    private ReadOnlyList() { }

}



[JsonObject]
public class ReadOnlyList<T> : IEnumerable<T> {

    [JsonProperty]
    private readonly T[] Collection;

    [JsonIgnore]
    public int Count => Collection.Length;

    [JsonIgnore]
    public T this[int index] => Collection[index];



    public static readonly ReadOnlyList<T> Empty = new();
    public static implicit operator ReadOnlyList<T>(ReadOnlyList empty) {

        if (empty != ReadOnlyList.Empty) {
            throw new ArgumentException($"This casting operator should only be used with {nameof(ReadOnlyList.Empty)}", nameof(empty));
        }

        return Empty;
    }



    private ReadOnlyList() {
        Collection = Array.Empty<T>();
    }

    public ReadOnlyList(IEnumerable<T> collection) {
        Collection = collection.ToArray();
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



    public IEnumerator<T> GetEnumerator() {
        return ((IEnumerable<T>)Collection).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public void CopyTo(T[] array, int arrayIndex) {
        Collection.CopyTo(array, arrayIndex);
    }

}