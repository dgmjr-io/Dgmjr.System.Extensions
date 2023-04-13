/*
 * System.Linq.Async.cs
 *
 *   Created: 2023-01-11-03:08:07
 *   Modified: 2023-01-13-10:39:32
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Linq;

internal static class EnumerableExtensions
{
    // public static async Task<IImmutableList<T>> ToImmutableListAsync<T>(this IEnumerable<T> e)
    //     => await Task.Factory.StartNew<IImmutableList<T>>(() => e.ToImmutableList());

    // public static async Task<ImmutableArray<T>> ToImmutableArrayAsync<T>(this IEnumerable<T> e)
    //     => await Task.Factory.StartNew(() => e.ToImmutableArray());
}
