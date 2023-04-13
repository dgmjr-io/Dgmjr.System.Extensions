/*
 * StringExtensions.cs
 *
 *   Created: 2022-11-11-06:06:01
 *   Modified: 2022-11-14-04:11:14
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;

namespace System;

public static class StringExtensions
{
    /// <summary>
    /// Escapes special characters in a string
    /// </summary>
    /// <param name="str">The string to escape</param>
    /// <returns>The escaped string</returns>
    public static string Escape(this string str)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));
        return str.Replace("&", @"\x26").Replace("<", @"\x3c").Replace(">", @"\x3e").Replace("\"", @"\x22").Replace("'", @"\x27");
    }

    /// <summary>
    /// Determines if a string is null or whitespace
    /// </summary>
    /// <param name="str">The string to see whether it's null or whitespace
    ///     </param>
    /// <returns>A <see langword="bool"/> value indicating whether the string
    ///     was null or whitespace</returns>
    public static bool IsNullOrWhitespace(this string? str)
        => IsNullOrWhiteSpace(str);

    /// <summary>
    /// Determines if a string is null or empty
    /// </summary>
    /// <param name="str">The string to see whether it's null or empty</param>
    /// <returns>A <see langword="bool"/> value indicating whether the string
    ///     was null or empty</returns>
    public static bool IsNullOrEmpty(this string? str)
        => string.IsNullOrEmpty(str);

    public static byte[] FromBase64String(this string s)
        => Convert.FromBase64String(s);

    public static string ToBase64String(this byte[] b)
        => Convert.ToBase64String(b);


#if NETSTANDARD
    public static bool Contains(
        this string source,
        string toCheck,
        StringComparison comp
    ) => source?.IndexOf(toCheck, comp) >= 0;
#endif
}
