/*
 * System.Linq.Async.cs
 *
 *   Created: 2023-01-11-03:08:07
 *   Modified: 2023-01-13-10:39:32
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
#if !NETSTANDARD
namespace System.Linq;

using System.Collections.Immutable;
using System.Threading.Tasks;

internal static class EnumerableExtensions
{
    public static async Task<IImmutableList<T>> ToImmutableListAsync<T>(this IEnumerable<T> e) =>
        await Task.Factory.StartNew<IImmutableList<T>>(() => e.ToImmutableList());

    public static async Task<ImmutableArray<T>> ToImmutableArrayAsync<T>(this IEnumerable<T> e) =>
        await Task.Factory.StartNew(() => e.ToImmutableArray());
}
#endif
