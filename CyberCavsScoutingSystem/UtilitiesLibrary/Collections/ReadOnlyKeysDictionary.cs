using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace UtilitiesLibrary.Collections;



public class ReadOnlyKeysDictionary<TKey, TValue> :
    IReadOnlyDictionary<TKey, TValue>,
    IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{

    private readonly Dictionary<TKey, TValue> BackingDictionary;

    public TValue this[TKey key]
    {
        get => BackingDictionary[key];
        set => BackingDictionary[key] = value;
    }

    public int Count => BackingDictionary.Count;

    public IEnumerable<TKey> Keys => BackingDictionary.Keys.ToReadOnly();
    public IEnumerable<TValue> Values => BackingDictionary.Values.ToReadOnly();



    public ReadOnlyKeysDictionary()
    {
        BackingDictionary = new();
    }

    public ReadOnlyKeysDictionary(IDictionary<TKey, TValue> dictionary)
    {
        BackingDictionary = new(dictionary);
    }

    public ReadOnlyKeysDictionary(IEnumerable<TKey> keys, TValue defaultValue)
    {

        BackingDictionary = new();

        foreach (TKey key in keys)
        {
            BackingDictionary.Add(key, defaultValue);
        }
    }



    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return BackingDictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return BackingDictionary.GetEnumerator();
    }



    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return BackingDictionary.Contains(item);
    }

    public bool ContainsKey(TKey key)
    {
        return BackingDictionary.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        return BackingDictionary.ContainsValue(value);
    }



    public bool TryGetValue(TKey key, out TValue? value)
    {
        return BackingDictionary.TryGetValue(key, out value);
    }

}