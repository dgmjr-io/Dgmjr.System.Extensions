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

using System.Threading.Tasks;

namespace System.IO;

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
}