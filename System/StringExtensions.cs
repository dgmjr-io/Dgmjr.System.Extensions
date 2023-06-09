using System;
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
        return str.Replace("&", @"\x26")
            .Replace("<", @"\x3c")
            .Replace(">", @"\x3e")
            .Replace("\"", @"\x22")
            .Replace("'", @"\x27");
    }

    /// <summary>
    /// Determines if a string is null or whitespace
    /// </summary>
    /// <param name="str">The string to see whether it's null or whitespace
    ///     </param>
    /// <returns>A <see langword="bool"/> value indicating whether the string
    ///     was null or whitespace</returns>
    public static bool IsNullOrWhitespace(this string? str) => string.IsNullOrWhiteSpace(str);

    /// <summary>
    /// Determines if a string is null or empty
    /// </summary>
    /// <param name="str">The string to see whether it's null or empty</param>
    /// <returns>A <see langword="bool"/> value indicating whether the string
    ///     was null or empty</returns>
    public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// This is a C# extension method that converts a Base64-encoded string to a byte array.
    /// </summary>
    /// <param name="s">The input string that represents a base64 encoded <see langword="byte" /> array.</param>
    /// <returns>A <see langword="byte" /> array representing the decoded base64 <see langword="string" /></returns>
    public static byte[] FromBase64String(this string s) => Convert.FromBase64String(s);

    /// <summary>
    /// This is a C# extension method that converts a byte array to a Base64 string.
    /// </summary>
    /// <param name="b">The parameter "b" is a byte array that represents the data to be converted to a
    /// Base64 string.</param>
    /// <returns>The <see langword="byte" /> array as a base64 encoded <see langword="string" /></returns>
    public static string ToBase64String(this byte[] b) => Convert.ToBase64String(b);

#if NETSTANDARD
    /// <summary>
    /// **Polyfill for NETSTANDARD**
    /// The function checks if a string contains a specified substring using a specified comparison method,
    /// and returns a boolean value.
    /// </summary>
    /// <param name="source">A string variable representing the source string in which we want to check if
    /// another string is present or not.</param>
    /// <param name="toCheck">The string that we want to check if it exists within the source
    /// string.</param>
    /// <param name="comp">comp is an optional parameter of type <see cref="SStringComparison" /> <see langword="enum" />. It specifies the
    /// type of comparison to use when comparing the source <see langword="string" /> and the <paramref name="toCheck" /> <see langword="string" />. If it is not
    /// provided, the default value is <see cref="StringComparison.OrdinalIgnoreCase" />, which performs a case-insensitive
    /// comparison. Other possible values for <see cref="SStringComparison" /> include <see cref="SStringComparison.CurrentCulture" />
    /// </param>
    public static bool Contains(
        this string source,
        string toCheck,
        StringComparison? comp = null
    ) => source?.IndexOf(toCheck, comp ?? StringComparison.OrdinalIgnoreCase) >= 0;
#endif
}
