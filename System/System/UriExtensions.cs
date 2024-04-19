/*
 * UriExtensions.cs
 *     Created: 2023-46-22T02:46:39-05:00
 *    Modified: 2024-31-19T13:31:02-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public static class UriExtensions
{
    public static Stream OpenRead(this Uri uri, HttpClient? httpClient = default) =>
        (httpClient ?? new()).GetStreamAsync(uri).Result;

    public static async Task<Stream> OpenReadAsync(
        this Uri uri,
        HttpClient? httpClient = default
    ) => await (httpClient ?? new HttpClient()).GetStreamAsync(uri);

    public static byte[] DownloadBytes(this Uri uri, HttpClient? httpClient = default) =>
        uri.DownloadBytesAsync(httpClient).Result;

    public static async Task<byte[]> DownloadBytesAsync(
        this Uri uri,
        HttpClient? httpClient = default
    ) => await (httpClient ?? new HttpClient()).GetByteArrayAsync(uri);

    public static string DownloadString(this Uri uri, HttpClient? httpClient = default) =>
        (httpClient ?? new HttpClient()).GetStringAsync(uri).Result;

    public static async Task<string> DownloadStringAsync(
        this Uri uri,
        HttpClient? httpClient = default
    ) => await (httpClient ?? new HttpClient()).GetStringAsync(uri);
}
