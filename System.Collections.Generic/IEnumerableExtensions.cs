/*
 * IEnumerableExtensions.cs
 *
 *   Created: 2022-12-27-08:55:30
 *   Modified: 2022-12-27-08:55:30
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

#if !DGMJRS_IENUMERABLE_EXTENSIONS
#define DGMJRS_IENUMERABLE_EXTENSIONS

namespace System.Collections.Generic;

public static class DgmjrsIEnumerableExtensions
{
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <returns>A sequence that contains only elements that are not null in the source sequence or an empty sequence if no elements are null</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class =>
        (source?.Where(item => item != null) ?? Empty<T>())!;

    /// <summary>
    /// Concatenates the elements of a sequence into a single string using the specified separator between each element.
    /// </summary>
    /// <exception cref="ArgumentException"> The sequence is empty or cannot be converted to a string</exception>
    /// <param name="source">The source enumeration</param>
    /// <param name="separator">The string to use as a separator. System. String. Empty is treated as the empty string.</param>
    /// <returns>A string that consists of the elements in delimited by the string. If is an empty sequence the method returns the string</returns>
    public static string Join(this IEnumerable source, string separator) =>
        string.Join(separator, source.OfType<object>());
}
#endif
