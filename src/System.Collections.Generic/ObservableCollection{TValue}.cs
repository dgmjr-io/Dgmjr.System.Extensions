/*
 * ObservableCollection{TValue}.cs
 *
 *   Created: 2023-02-01-03:58:10
 *   Modified: 2023-02-01-03:58:10
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Collections.Generic;

public class ObservableCollection<TValue> : ICollection<TValue>
{
    private readonly Action<TValue> _onAdd = v => { };
    private readonly Action<TValue> _onRemove = v => { };
    private readonly List<TValue> _list = new();

    public virtual void OnAdd(TValue value) => _onAdd?.Invoke(value);

    public virtual void OnRemove(TValue value) => _onRemove?.Invoke(value);

    public ObservableCollection()
        : this(Empty<TValue>(), v => { }, v => { }) { }

    public ObservableCollection(Action<TValue>? onAdd = null, Action<TValue>? onRemove = null)
        : this(Empty<TValue>(), onAdd, onRemove) { }

    public virtual void Clear()
    {
        foreach (var item in _list)
            OnRemove(item);
        _list.Clear();
    }

    public virtual bool Contains(TValue item) => _list.Contains(item);

    public virtual void CopyTo(TValue[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public virtual bool Remove(TValue item)
    {
        var result = _list.Remove(item);
        if (result)
            OnRemove(item);
        return result;
    }

    public virtual IEnumerator<TValue> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual void Add(TValue item)
    {
        _list.Add(item);
        OnAdd(item);
    }

    public virtual int Count => _list.Count;

    public virtual bool IsReadOnly => false;

    public virtual int IndexOf(TValue item) => _list.IndexOf(item);

    public virtual void Insert(int index, TValue item)
    {
        _list.Insert(index, item);
        OnAdd(item);
    }

    public ObservableCollection(
        IEnumerable<TValue> collection,
        Action<TValue>? onAdd = null,
        Action<TValue>? onRemove = null
    )
    {
        _onAdd ??= onAdd;
        _onRemove ??= onRemove;
        foreach (var item in collection)
            Add(item);
    }
}
