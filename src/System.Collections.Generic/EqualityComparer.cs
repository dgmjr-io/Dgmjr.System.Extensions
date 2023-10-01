//
// EqualityComparer.cs
//
//   Created: 2022-10-23-08:36:04
//   Modified: 2022-10-30-08:56:53
//
//   Author: David G. Mooore, Jr. <david@dgmjr.io>
//
//   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
//

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

using System;

public sealed class LambdaEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _comparer;
    private readonly Func<T, int> _getHashCode;
    private static readonly Func<T, int> _defaultGetHashCode = t => HashCode.Combine(t);

    public LambdaEqualityComparer(Comparer<T> comparer, Func<T, int>? getHashCode = default)
        : this((x, y) => comparer.Compare(x, y) == 0, getHashCode) { }

    public LambdaEqualityComparer(Func<T, T, bool> comparer, Func<T, int>? getHashCode = default)
    {
        _comparer = comparer;
        _getHashCode = getHashCode ?? _defaultGetHashCode;
    }

    public bool Equals(T? x, T? y) => _comparer(x, y);

    public int GetHashCode( /*[DisallowNull]*/
        T obj
    ) => _getHashCode(obj);
}
