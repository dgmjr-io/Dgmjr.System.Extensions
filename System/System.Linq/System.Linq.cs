/*
 * System.Linq.cs
 *
 *   Created: 2022-10-23-11:19:40
 *   Modified: 2022-11-11-10:26:36
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Linq;

// #if DEFINE_INTERNAL
public static class DmjrsLinqExtensions
// #else
// #endif
{
    /// <summary>
    /// Determines if the <see cref="IEnumerable{T}"/> is null or empty.
    /// </summary>
    /// <param name="e">The <see cref="IEnumerable{T}"/> to check</param>
    /// <typeparam name="T">The type of elements in the
    ///     <see cref="IEnumerable{T}"/></typeparam>
    /// <returns><c>TRUE</c> if <paramref name="e"/> is <c>NULL</c> or empty,
    ///     <c>FALSE</c> otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> e) =>
        e is ICollection<T> collection ? collection.Count == 0 : !e?.Any() ?? true;

    /// <summary>
    /// Performs the specified action on each element of the
    /// <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="e">The <see cref="IEnumerable{T}"/> to perform the action
    ///     on.</param>
    /// <param name="foreach">The action to perform on each element of the
    ///     <see cref="IEnumerable{T}"/>.</param>
    public static void ForEach<T>(this IEnumerable<T> e, Action<T> @foreach)
    {
        _ = e.Aggregate(0, (i, item) =>
        {
            @foreach(item);
            return i + 1;
        });
    }

    /// <summary>
    /// Adds the specified elements to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="collection">The <see cref="ICollection{T}"/> to add the
    ///     elements to.</param>
    /// <param name="thingsToAdd">The elements to add to the
    ///     <see cref="ICollection{T}"/>.</param>
    /// <typeparam name="TCollection">The type of the
    ///     <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="T">The type of elements in the
    ///     <see cref="ICollection{T}"/>.</typeparam>
    /// <returns>The <see cref="ICollection{T}"/> with the added elements.
    ///     </returns>
    public static TCollection AddRange<TCollection, T>(
        this TCollection collection,
        IEnumerable<T> thingsToAdd
    )
        where TCollection : ICollection<T>
    {
        if (thingsToAdd != null)
        {
            if (collection is IList<T> list)
            {
                list.AddRange(thingsToAdd);
            }
            else
            {
                thingsToAdd.ForEach(item => collection.Add(item));
            }
        }
        return collection;
    }

    /// <summary>Removes the specified elements from the <see cref="ICollection{T}"/>.</summary>
    /// <param name="collection">The collection from which to remove the elements</param>
    /// <param name="removeRange">The elements to remove from the <see cref="ICollection{T}"/></param>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <returns>The <see cref="ICollection{T}"/> with the removed elements.</returns>
    public static TCollection RemoveRange<TCollection, T>(
        this TCollection collection,
        IEnumerable<T> removeRange
    )
        where TCollection : ICollection<T>
    {
        if (removeRange != null)
        {
            // if (collection is List<T> list)
            // {
            //     list.RemoveRange(removeRange);
            // }
            // else
            // {
            removeRange.ForEach(item => collection.Remove(item));
            // }
        }
        return collection;
    }

    /// <summary>
    /// Removes the elements from the collection that match the specified predicate.
    /// **MUTATES** the collection!!
    /// </summary>
    /// <param name="collection">The collection from which to remove the elements.</param>
    /// <param name="predicate">The predicate to match the elements to remove. </param>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <returns>The <see cref="ICollection{T}"/> with the removed elements.</returns>
    public static TCollection Without<TCollection, T>(
        this TCollection collection,
        Func<T, bool> predicate
    )
        where TCollection : ICollection<T>
    {
        var itemsToRemove = collection.Where(predicate);
        collection.RemoveRange(itemsToRemove);
        return collection;
    }

    /// <summary>
    /// Returns all elements from <paramref name="collection"/> that <b>don't</b> match the specified predicate.
    /// </summary>
    /// <param name="collection">The collection from which to select the elements.</param>
    /// <param name="predicate">The predicate to match the elements to not select.</param>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <returns>The <see cref="ICollection{T}"/> without the matchings elements.</returns>
    public static TCollection Except<TCollection, T>(
        this TCollection collection,
        Func<T, bool> predicate
    )
        where TCollection : ICollection<T> =>
        (TCollection)
            Activator.CreateInstance(
                typeof(TCollection),
                collection.Where(x => !predicate(x)).ToList()
            )!;

    /// <summary>
    /// This function appends a new value to an existing IEnumerable collection and returns the updated
    /// collection.
    /// </summary>
    /// <param name="values">An IEnumerable of type T representing the collection of values to which <paramref name="newValue" />  value will be appended.</param>
    /// <param name="newValue">A new value to append to the <paramref name="values" /></param>
    /// <typeparam name="T">T is a generic type parameter that can be replaced with any type at runtime. It is
    /// used to define the type of elements in the input IEnumerable and the type of the new value to be
    /// appended to it.</typeparam>
    public static IEnumerable<T> Append<T>(IEnumerable<T> values, T newValue)
    {
        foreach (var value in values)
        {
            yield return value;
        }
        yield return newValue;
    }

    /// <summary>
    /// The function adds a new value to the beginning of an existing sequence and returns the updated
    /// sequence.
    /// </summary>
    /// <param name="values">An IEnumerable of type T representing the collection of values to which a new
    /// value will be prepended.</param>
    /// <param name="newValue">A new value to prepend to the <paramref name="values" /></param>
    /// <typeparam name="T">T is a generic type parameter that can be replaced with any type at runtime. It
    /// represents the type of elements in the input sequence and the type of the new value being added to
    /// the beginning of the sequence.</typeparam>
    public static IEnumerable<T> Prepend<T>(IEnumerable<T> values, T newValue)
    {
        yield return newValue;
        foreach (var value in values)
        {
            yield return value;
        }
    }

    /// <summary>
    /// Returns the greatest power of two that is less than or equal to the maximum value in the collection of integers.
    /// </summary>
    /// <param name="values">The collection of integers.</param>
    /// <returns>The greatest power of two.</returns>
    public static int GreatestPowerOfTwo(this IEnumerable<int> values)
    {
        var n = values.Max();
        var k = 1;
        while (k < n)
        {
            k <<= 1;
        }
        return k;
    }

    /// <summary>
    /// Returns the greatest power of two that is less than or equal to the maximum value obtained by applying a selector function to each element in the collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="values">The collection of elements.</param>
    /// <param name="selector">A function to extract an integer value from each element.</param>
    /// <returns>The greatest power of two.</returns>
    public static int GreatestPowerOfTwo<T>(this IEnumerable<T> values, Func<T, int> selector)
    {
        var n = values.Max(selector);
        var k = 1;
        while (k < n)
        {
            k <<= 1;
        }
        return k;
    }

    public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IAsyncEnumerable<T> asyncEnumerable)
    {
        var list = new List<T>();
        await foreach (var item in asyncEnumerable)
        {
            list.Add(item);
        }
        return list;
    }

    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> enumerable, int count = 1, bool allowDuplicates = false)
    {
#if !NET5_0_OR_GREATER
        var random = new Random();
#endif
        var list = enumerable.ToList();
        for (var i = 0; i < count; i++)
        {
#if NET5_0_OR_GREATER
            var index = Random.Shared.Next(0, list.Count);
#else
            var index = random.Next(0, list.Count);
#endif
            yield return list[index];
            if (!allowDuplicates)
            {
                list.RemoveAt(index);
            }
        }
    }

    public static T TakeRandom<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.TakeRandom(1).First();
    }
}
