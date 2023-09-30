// /*
//  * TryCreateUri.cs
//  *
//  *   Created: 2022-12-17-12:29:27
//  *   Modified: 2022-12-17-12:29:27
//  *
//  *   Author: David G. Mooore, Jr. <david@dgmjr.io>
//  *
//  *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
//  *      License: MIT (https://opensource.org/licenses/MIT)
//  */

// namespace System;

// internal static class TryCreateUriExtensions
// {
//     public static bool TryCreateUri(this string uriString, out Uri? uri)
//     {
//         try { uri = uriString.CreateUri(true); return true; }
//         catch { uri = null; return false; }
//     }

//     public static Uri? CreateUri(this string uriString, bool throwOnInvalidUri = true)
//     {
//         if (string.IsNullOrEmpty(uriString))
//             throw new ArgumentException("The provided URI string is null or empty.", nameof(uriString));
//         if (Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
//             return uri;
//         if (throwOnInvalidUri)
//             throw new ArgumentException("The provided string is not a valid URI.", nameof(uriString));
//         return null;
//     }

//     public static Uri CreateUri(this string uriString, string defaultFallbackUri)
//     {
//         if (string.IsNullOrEmpty(defaultFallbackUri))
//             throw new ArgumentException("The provided default fallback URI is null or empty.", nameof(defaultFallbackUri));
//         if (string.IsNullOrEmpty(uriString))
//             throw new ArgumentException("The provided URI string is null or empty.", nameof(uriString));

//         if (System.Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
//             return uri;
//         return new(string.Format(defaultFallbackUri, uriString));
//     }
// }