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
