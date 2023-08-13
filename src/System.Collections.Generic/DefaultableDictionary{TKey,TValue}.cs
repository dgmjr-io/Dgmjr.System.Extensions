/*
 * DefaultableDictionary.cs
 *
 *   Created: 2022-12-23-05:20:21
 *   Modified: 2022-12-23-05:20:21
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

#if !DEFAULTABLE_DICTIONARY
#define DEFAULTABLE_DICTIONARY
namespace System.Collections.Generic;

///<summary>
///A wrapper around a <see cref="Dictionary{TKey, TValue}"/> that allows for a
///default value to be returned when a key is not found rather than throwing an
///exception.
///</summary>
///<typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
///<typeparam name="TValue">The type of the values in the dictionary.
///    </typeparam>
///<remarks>
///When using this class, you should be aware that the
///<see cref="TryGetValue(TKey, out TValue)"/> method will always return
///<see langword="true"/> since it will return the <see cref="DefaultValue"/> if
///the key does not exist in the dictionary. To check if the key exists in the
///dictionary, use the <see cref="ContainsKey(TKey)"/> method instead.
///</remarks>
///<example>
///<code>
///         var dictionary = new DefaultableDictionary&lt;string, int&gt;();
///         dictionary.Add("one", 1);
///         dictionary.Add("two", 2);
///         dictionary.Add("three", 3);
///
///         int one = dictionary["one"]; // returns 1
///         int two = dictionary["two"]; // returns 2
///         int three = dictionary["three"]; // returns 3
///         int four = dictionary["four"]; // returns 0 (the default value)
///     </code>
///</example>
public class DefaultableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// The default <typeparamref name="TValue"/> to return when a key is not
    /// found in the dictionary.
    /// </summary>
    /// <value>The default <typeparamref name="TValue"/> to return when a key is
    ///     not found in the dictionary.</value>
    /// <remarks>
    /// This value is set in the constructor and cannot be changed after the
    /// object is created.
    /// </remarks>
    public TValue DefaultValue { get; init; }
    private readonly Dictionary<TKey, TValue> _dictionary = new();
    public DefaultableDictionary() : this(default(TValue)) { }
    public DefaultableDictionary(IDictionary<TKey, TValue> original) : this(default, original, EqualityComparer<TKey>.Default) { }
    public DefaultableDictionary(IDictionary<TKey, TValue> original, IEqualityComparer<TKey> keyComparer) : this(default, original, keyComparer) { }
    public DefaultableDictionary(TValue defaultValue) : this(defaultValue, EqualityComparer<TKey>.Default) { }
    public DefaultableDictionary(TValue defaultValue, IEqualityComparer<TKey> keyComparer) : this(defaultValue, new Dictionary<TKey, TValue>(), EqualityComparer<TKey>.Default) { }
    public DefaultableDictionary(TValue defaultValue, IDictionary<TKey, TValue> original, IEqualityComparer<TKey> keyComparer)
    {
        this.DefaultValue = defaultValue;
        this._dictionary = new Dictionary<TKey, TValue>(original, keyComparer);
    }
    public virtual ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)this._dictionary).Keys;

    public virtual ICollection<TValue> Values => ((IDictionary<TKey, TValue>)this._dictionary).Values;

    public virtual int Count => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Count;

    public virtual bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).IsReadOnly;

    public virtual TValue this[TKey key] { get => _dictionary.TryGetValue(key, out var value) ? value : DefaultValue; set => _dictionary[key] = value; }

    public virtual void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>)this._dictionary).Add(key, value);
    public virtual bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>)this._dictionary).ContainsKey(key);
    public virtual bool Remove(TKey key) => ((IDictionary<TKey, TValue>)this._dictionary).Remove(key);
    public virtual bool TryGetValue(TKey key, out TValue value)
    {
        value = ((IDictionary<TKey, TValue>)this._dictionary).TryGetValue(key, out value) ? value : DefaultValue;
        return true;
    }
    public virtual void Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Add(item);
    public virtual void Clear() => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Clear();
    public virtual bool Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Contains(item);
    public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).CopyTo(array, arrayIndex);
    public virtual bool Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)this._dictionary).Remove(item);
    public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)this._dictionary).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this._dictionary).GetEnumerator();
}
#endif
