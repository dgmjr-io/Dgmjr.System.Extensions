/*
 * StringExtensions.cs
 *
 *   Created: 2022-11-11-06:06:01
 *   Modified: 2022-11-14-04:11:14
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.Encodings.Web;

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
    public static bool IsNullOrWhitespace(this string? str) => IsNullOrWhiteSpace(str);

    /// <summary>
    /// Determines if a string is null or empty
    /// </summary>
    /// <param name="str">The string to see whether it's null or empty</param>
    /// <returns>A <see langword="bool"/> value indicating whether the string
    ///     was null or empty</returns>
    public static bool IsNullOrEmpty(this string? str) => IsNullOrEmpty(str);

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

    public static string FormatIfNotNullOrEmpty(
        this string? testString,
        string formatString = "{0}"
    ) => !IsNullOrEmpty(testString) ? Format(formatString, testString) : string.Empty;

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
    /// <param name="comp">comp is an optional parameter of type <see cref="StringComparison" /> <see langword="enum" />. It specifies the
    /// type of comparison to use when comparing the source <see langword="string" /> and the <paramref name="toCheck" /> <see langword="string" />. If it is not
    /// provided, the default value is <see cref="StringComparison.OrdinalIgnoreCase" />, which performs a case-insensitive
    /// comparison. Other possible values for <see cref="StringComparison" /> include <see cref="StringComparison.CurrentCulture" />
    /// </param>
    public static bool Contains(
        this string source,
        string toCheck,
        StringComparison? comp = null
    ) => source?.IndexOf(toCheck, comp ?? StringComparison.OrdinalIgnoreCase) >= 0;
#endif

    /// <summary>
    /// Concatentates many strings into one using a <paramref name="separator" />
    /// </summary>
    /// <param name="separator">The string to be used between the values in <paramref name="values" /></param>
    /// <param name="values">The list of values to be converted to a concatenated string</param>
    /// <returns>The concatenated string</returns>
    public static string Join(string separator, IEnumerable values) =>
        string.Join(separator, values.OfType<object>());

    /// <summary>
    /// Checks if the given string is null, empty, or consists only of whitespace characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the string is null, empty, or consists only of whitespace characters; otherwise, false.</returns>
    [DebuggerStepThrough]
    public static bool IsMissing(this string value)
    {
        return IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Checks if the given string is null, empty, consists only of whitespace characters, or exceeds a specified maximum length.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="maxLength">The maximum length allowed.</param>
    /// <returns>true if the string is null, empty, consists only of whitespace characters, or exceeds the specified maximum length; otherwise, false.</returns>
    [DebuggerStepThrough]
    public static bool IsMissingOrTooLong(this string value, int maxLength)
    {
        return IsNullOrWhiteSpace(value) || value.Length > maxLength;
    }

    /// <summary>
    /// Checks if the given string is not null, not empty, and contains at least one non-whitespace character.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the string is not null, not empty, and contains at least one non-whitespace character; otherwise, false.</returns>
    [DebuggerStepThrough]
    public static bool IsPresent(this string value)
    {
        return !IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Ensures that the URL has a leading forward slash (/) character. If the URL already starts with a forward slash, it is returned unchanged.
    /// </summary>
    /// <param name="url">The URL to modify.</param>
    /// <returns>The URL with a leading forward slash (/) character.</returns>
    [DebuggerStepThrough]
    public static string? EnsureLeadingSlash(this string url)
    {
        return url?.StartsWith("/") == false ? "/" + url : url;
    }

    /// <summary>
    /// Ensures that the URL has a trailing forward slash (/) character. If the URL already ends with a forward slash, it is returned unchanged.
    /// </summary>
    /// <param name="url">The URL to modify.</param>
    /// <returns>The URL with a trailing forward slash (/) character.</returns>
    [DebuggerStepThrough]
    public static string? EnsureTrailingSlash(this string url)
    {
        return url?.EndsWith("/") == false ? url + "/" : url;
    }

    /// <summary>
    /// Removes the leading forward slash (/) character from the URL, if it exists.
    /// </summary>
    /// <param name="url">The URL to modify.</param>
    /// <returns>The URL with the leading forward slash (/) character removed, or the original URL if it doesn't start with a forward slash.</returns>
    [DebuggerStepThrough]
    public static string? RemoveLeadingSlash(this string url)
    {
        if (url?.StartsWith("/") == true)
        {
            url = url.Substring(1);
        }

        return url;
    }

    /// <summary>
    /// Removes the trailing forward slash (/) character from the URL, if it exists.
    /// </summary>
    /// <param name="url">The URL to modify.</param>
    /// <returns>The URL with the trailing forward slash (/) character removed, or the original URL if it doesn't end with a forward slash.</returns>
    [DebuggerStepThrough]
    public static string? RemoveTrailingSlash(this string url)
    {
        if (url?.EndsWith("/") == true)
        {
            url = url.Substring(0, url.Length - 1);
        }

        return url;
    }

    /// <summary>
    /// Cleans the URL path by replacing a null or empty URL with a forward slash (/) character,
    /// and removing the trailing forward slash (/) character, if it exists and the URL is not equal to "/".
    /// </summary>
    /// <param name="url">The URL to clean.</param>
    /// <returns>The cleaned URL.</returns>
    [DebuggerStepThrough]
    public static string CleanUrlPath(this string url)
    {
        if (IsNullOrWhiteSpace(url))
            url = "/";

        if (url != "/" && url.EndsWith("/"))
        {
            url = url.Substring(0, url.Length - 1);
        }

        return url;
    }

    /// <summary>
    /// Checks if the given URL is a local URL.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>True if the URL is local; otherwise, false.</returns>
    [DebuggerStepThrough]
    public static bool IsLocalUrl(this string url)
    {
        if (IsNullOrEmpty(url))
        {
            return false;
        }

        // Allows "/" or "/foo" but not "//" or "/\".
        if (url[0] == '/')
        {
            // url is exactly "/"
            if (url.Length == 1)
            {
                return true;
            }

            // url doesn't start with "//" or "/\"
            return url[1] != '/' && url[1] != '\\';
        }

        // Allows "~/" or "~/foo" but not "~//" or "~/\".
        if (url[0] == '~' && url.Length > 1 && url[1] == '/')
        {
            // url is exactly "~/"
            if (url.Length == 2)
            {
                return true;
            }

            // url doesn't start with "~//" or "~/\"
            return url[2] != '/' && url[2] != '\\';
        }

        return false;
    }

    /// <summary>
    /// Adds a query string to the given URL.
    /// </summary>
    /// <param name="url">The URL to add the query string to.</param>
    /// <param name="query">The query string to add.</param>
    /// <returns>The modified URL with the added query string.</returns>
    [DebuggerStepThrough]
    public static string AddQueryString(this string url, string query)
    {
        if (!url.Contains('?'))
        {
            url += "?";
        }
        else if (!url.EndsWith("&"))
        {
            url += "&";
        }

        return url + query;
    }

    /// <summary>
    /// Adds a query string parameter to the given URL.
    /// </summary>
    /// <param name="url">The URL to add the query string parameter to.</param>
    /// <param name="name">The name of the query string parameter.</param>
    /// <param name="value">The value of the query string parameter.</param>
    /// <returns>The modified URL with the added query string parameter.</returns>
    [DebuggerStepThrough]
    public static string AddQueryString(this string url, string name, string value)
    {
        return url.AddQueryString(name + "=" + UrlEncoder.Default.Encode(value));
    }

    /// <summary>
    /// Adds a hash fragment to the given URL.
    /// </summary>
    /// <param name="url">The URL to add the hash fragment to.</param>
    /// <param name="query">The hash fragment to add.</param>
    /// <returns>The modified URL with the added hash fragment.</returns>
    [DebuggerStepThrough]
    public static string AddHashFragment(this string url, string query)
    {
        if (!url.Contains('#'))
        {
            url += "#";
        }

        return url + query;
    }

    private const string Http = "http";
    private const string Https = "https";

    /// <summary>
    /// Retrieves the origin of the given URL.
    /// </summary>
    /// <param name="url">The URL to retrieve the origin from.</param>
    /// <returns>The origin part of the URL (scheme and authority), or null if the URL is invalid or not using HTTP or HTTPS scheme.</returns>
    public static string? GetOrigin(this string url)
    {
        if (url != null)
        {
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (Exception)
            {
                return null;
            }

            if (uri.Scheme == Http || uri.Scheme == Https)
            {
                return $"{uri.Scheme}://{uri.Authority}";
            }
        }

        return null;
    }

    /// <summary>
    /// Obfuscates the given string value by replacing all characters except the last four characters with asterisks.
    /// </summary>
    /// <param name="value">The string value to obfuscate.</param>
    /// <returns>The obfuscated string value.</returns>
    public static string Obfuscate(this string value)
    {
        var last4Chars = "****";
        if (value.IsPresent() && value.Length > 4)
        {
            last4Chars = value.Substring(0, value.Length - 4);
        }

        return "****" + last4Chars;
    }

    /// <summary>
    /// Checks if the given string value is a valid URI.
    /// </summary>
    /// <param name="value">The string value to check.</param>
    /// <returns>True if the string value is a valid URI; otherwise, false.</returns>
    public static bool IsUri(this string value)
    {
        return Uri.TryCreate(value, Absolute, out _);
    }
}
