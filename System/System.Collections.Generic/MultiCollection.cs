namespace System.Collections.Generic;

using System.Collections;
using System.Collections.ObjectModel;

public class MultiCollection<T>(params ICollection<T>[] collections) : ICollection<T>
{
    private readonly ICollection<T>[] _collections = collections;

    public int Count => _collections[0].Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        foreach(var collection in _collections)
        {
            collection.Add(item);
        }
    }

    public void Clear()
    {
        foreach(var collection in _collections)
        {
            collection.Clear();
        }
    }

    public bool Contains(T item)
    {
        return _collections[0].Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _collections[0].CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator() => _collections[0].GetEnumerator();

    public bool Remove(T item)
    {
        foreach(var collection in _collections)
        {
            collection.Remove(item);
        }
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
