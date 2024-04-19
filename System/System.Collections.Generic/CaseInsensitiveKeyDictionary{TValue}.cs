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
// Documentation for CaseInsensitiveKeyDictionary

namespace System.Collections.Generic; // Namespace declaration for the custom class

using System.Linq; // Using directive for LINQ functionality

public class CaseInsensitiveKeyDictionary<TValue> : Dictionary<string, TValue>
{
    /// <summary>
    /// Initializes a new instance of the CaseInsensitiveKeyDictionary class that is empty.
    /// </summary>
    public CaseInsensitiveKeyDictionary()
        : this(Enumerable.Empty<KeyValuePair<string, TValue>>()) { }

    /// <summary>
    /// Initializes a new instance of the CaseInsensitiveKeyDictionary class with the specified original key-value pairs.
    /// </summary>
    /// <param name="original">The original key-value pairs to initialize the dictionary with.</param>
    public CaseInsensitiveKeyDictionary(IEnumerable<KeyValuePair<string, TValue>> original)
        : base(
            original.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            StringComparer.OrdinalIgnoreCase
        )
    {
        // Calls the base class constructor (Dictionary) to populate the dictionary
        // Key comparison is done in a case-insensitive manner using StringComparer.OrdinalIgnoreCase
    }
}
