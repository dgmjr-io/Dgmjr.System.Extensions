/*
 * StreamExtensions.cs
 *
 *   Created: 2023-07-28-02:33:24
 *   Modified: 2023-07-28-02:33:24
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.IO;

using System.Threading.Tasks;

public static class StreamExtensions
{
    /// <summary>
    /// Reads <inheritdoc cref="ReadToEnd" path="/returns" />
    /// </summary>
    /// <param name="s">the stream to read from</param>
    /// <returns> the contents of the <see cref="Stream" /> as a <see langword="string" /> to the end</returns>
    public static string ReadToEnd(this Stream s) => new StreamReader(s).ReadToEnd();

    /// <summary>
    /// Reads <inheritdoc cref="ReadToEnd" path="/returns" />, read asynchronously from the <see cref="Stream" />
    /// </summary>
    /// <param name="s">the stream to read from</param>
    /// <returns> the contents of the <see cref="Stream" /> as a <see langword="string" /> to the end</returns>
    public static Task<string> ReadToEndAsync(this Stream s) =>
        new StreamReader(s).ReadToEndAsync();

    /// <summary>
    /// Reads <inheritdoc cref="ReadAllBytes" path="/returns" />
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    /// <returns>all <see langword="byte" />s from the <paramref name="stream" /></returns>
    public static byte[] ReadAllBytes(this Stream stream)
    {
        var buff = new byte[stream.Length];
        stream.Read(buff, 0, buff.Length);
        return buff;
    }

    /// <summary>
    /// Reads <inheritdoc cref="ReadAllBytesAsync" path="/returns" />
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    /// <returns>all <see langword="byte" />s from the <paramref name="stream" /></returns>
    public static async Task<byte[]> ReadAllBytesAsync(this Stream stream)
    {
        var buff = new byte[stream.Length];
        await stream.ReadAsync(buff, 0, buff.Length);
        return buff;
    }
}
