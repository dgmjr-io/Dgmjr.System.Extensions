// using System;
// /*
//  * System.UriExtensions.cs
//  *
//  *   Created: 2022-10-24-06:07:13
//  *   Modified: 2022-11-14-04:11:06
//  *
//  *   Author: David G. Mooore, Jr. <david@dgmjr.io>
//  *
//  *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
//  *      License: MIT (https://opensource.org/licenses/MIT)
//  */

// namespace System;
// using @uri = System.Uri;

// // #if DEFINE_INTERNAL
// internal static class UriExtensions
// // #else
// // public static class UriExtensions
// // #endif
// {
//     public static uri? ToUri(this string uriString, bool throwOnInvalidUri = true)
//     {
//         if (string.IsNullOrEmpty(uriString))
//             throw new ArgumentException("The provided URI string is null or empty.", nameof(uriString));
//         if (Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
//             return uri;
//         if (throwOnInvalidUri)
//             throw new ArgumentException("The provided string is not a valid URI.", nameof(uriString));
//         return null;
//     }

//     public static uri ToUri(this string uriString, string defaultFallbackUri)
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