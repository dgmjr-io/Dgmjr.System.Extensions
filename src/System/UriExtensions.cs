namespace System;

using System.Net.Http;

public static class UriExtensions
{
    public static Stream OpenRead(this Uri uri) => new HttpClient().GetStreamAsync(uri).Result;
}
