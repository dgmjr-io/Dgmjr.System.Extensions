/*
 * System.Linq.cs
 *
 *   Created: 2022-10-23-11:19:40
 *   Modified: 2022-11-11-10:26:36
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Linq;

// #if DEFINE_INTERNAL
public static class DmjrsLinqExtensions
// #else
// public static class JustinsLinqExtensions
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
        foreach (var item in e)
            @foreach(item);
    }

    /// <summary>
    /// Adds the specified elements to the <see cref="ICollection{T}"/>.
    /// </summary>
    /// <param name="collection">The <see cref="ICollection{T}"/> to add the
    ///     elements to.</param>
    /// <param name="thingsToAdd">The elements to add to the
    ///     <see cref="ICollection{T}"/>.</param>
    /// <typeparam name="T">The type of elements in the
    ///     <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="TCollection">The type of the
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
                foreach (var item in thingsToAdd)
                {
                    collection.Add(item);
                }
            }
        }
        return collection;
    }

    /// <summary>Removes the specified elements from the <see cref="ICollection{T}"/>.</summary>
    /// <param name="collection">The collection from which to remove the elements</param>
    /// <param name="removeRange">The elements to remove from the <see cref="ICollection{T}"/></param>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
    /// <returns>The <see cref="ICollection{T}"/> with the removed elements.</returns>
    public static TCollection RemoveRange<TCollection, T>(
        this TCollection collection,
        IEnumerable<T> removeRange
    )
        where TCollection : ICollection<T>
    {
        if (removeRange != null)
        {
            if (collection is IList<T> list)
            {
                list.RemoveRange(removeRange);
            }
            else
            {
                foreach (var item in removeRange)
                {
                    collection.Remove(item);
                }
            }
        }
        return collection;
    }

    /// <summary>
    /// Removes the elements from the collection that match the specified predicate.
    /// **MUTATES** the collection!!
    /// </summary>
    /// <param name="collection">The collection from which to remove the elements.</param>
    /// <param name="predicate">The predicate to match the elements to remove. </param>
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
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
    /// <typeparam name="T">The type of elements in the <see cref="ICollection{T}"/>.</typeparam>
    /// <typeparam name="TCollection">The type of the <see cref="ICollection{T}"/>.</typeparam>
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
    /// <typeparm name="T">T is a generic type parameter that can be replaced with any type at runtime. It is
    /// used to define the type of elements in the input IEnumerable and the type of the new value to be
    /// appended to it.</typeparm>
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
    /// <typeparm name="T">T is a generic type parameter that can be replaced with any type at runtime. It
    /// represents the type of elements in the input sequence and the type of the new value being added to
    /// the beginning of the sequence.</typeparm>
    public static IEnumerable<T> Prepend<T>(IEnumerable<T> values, T newValue)
    {
        yield return newValue;
        foreach (var value in values)
        {
            yield return value;
        }
    }
}
