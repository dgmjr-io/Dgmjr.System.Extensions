/*
 * CaseInsensitiveKeyDictionary{TValue}.cs
 *
 *   Created: 2023-03-31-04:47:28
 *   Modified: 2023-03-31-04:47:28
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Linq;

namespace System.Collections.Generic;

public class CaseInsensitiveKeyDictionary<TValue>(
    IEnumerable<KeyValuePair<string, TValue>> original
) : Dictionary<string, TValue>(original.ToDictionary(kvp => kvp.Key, kvp => kvp.Value), StringComparer.OrdinalIgnoreCase)
{
    public CaseInsensitiveKeyDictionary()
        : this(Empty<KeyValuePair<string, TValue>>()) { }
}
