namespace System.Text;
using static System.Text.Encoding;
#pragma warning disable CS1574

/// <summary>
/// A collection of methods that expose the functionality of
/// <see cref="System.Text.Encoding"/>'s public static instance members
/// statically.
/// </summary>
// #if DEFINE_INTERNAL
public static class TextEncodingExtensions
// #else
// public static class TextEncodingExtensions
// #endif
{
    /// <summary>
    /// Calls <see cref="Encoding.UTF8"/>.GetBytes(<paramref name="s"/>)
    /// </summary>
    /// <param name="s">The string to encode</param>
    /// <returns>The UTF8 encoded byte array</returns>
    public static byte[] GetUTF8Bytes(string s) => UTF8.GetBytes(s);

    /// <inheritdoc cref="GetUTF8Bytes(string)"/>
    public static byte[] ToUTF8Bytes(this string s) => GetUTF8Bytes(s);

    /// <summary>
    /// Calls <see cref="Encoding.UTF8.GetString(byte[])"/>
    /// </summary>
    /// <param name="bytes">The byte array to decode</param>
    /// <returns>The UTF8 decoded string</returns>
    public static string GetUTF8String(byte[] bytes) => UTF8.GetString(bytes, 0, bytes.Length);

    /// <inheritdoc cref="GetUTF8String(byte[])"/>
    public static string ToUTF8String(this byte[] bytes) => GetUTF8String(bytes);

    /// <summary>
    /// Calls <see cref="Encoding.Unicode.GetBytes(s)"/>
    /// </summary>
    /// <param name="s">The string to encode</param>
    /// <returns>The Unicode encoded byte string</returns>
    public static byte[] GetUnicodeBytes(string s) => Encoding.Unicode.GetBytes(s);

    /// <inheritdoc cref="GetUnicodeBytes(string)"/>
    public static byte[] ToUnicodeBytes(this string s) => GetUnicodeBytes(s);

    /// <summary>
    /// Calls <see cref="Encoding.Unicode.GetString(byte[])"/>
    /// </summary>
    /// <param name="bytes">The byte array to decode</param>
    /// <returns>The Unicode decoded string</returns>
    public static string GetUnicodeString(byte[] bytes) =>
        Encoding.Unicode.GetString(bytes, 0, bytes.Length);

    /// <inheritdoc cref="GetUnicodeString(byte[])"/>
    public static string ToUnicodeString(this byte[] bytes) => GetUnicodeString(bytes);

#if NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// Calls <see cref="Encoding.ASCII.GetBytes(s)"/>
    /// </summary>
    /// <param name="s">The string to encode</param>
    /// <returns>The ASCII encoded byte string</returns>
    public static byte[] ToASCIIBytes(this string s) => GetASCIIBytes(s);

    /// <summary>
    /// Calls <see cref="Encoding.ASCII.GetString(byte[])"/>
    /// </summary>
    /// <param name="bytes">The byte array to decode</param>
    /// <returns>The ASCII decoded string</returns>
    public static string ToASCIIString(this byte[] bytes) => GetASCIIString(bytes);

    /// <summary>
    /// Calls <see cref="Encoding.ASCII.GetBytes(s)"/>
    /// </summary>
    /// <param name="s">The string to encode</param>
    /// <returns>The ASCII encoded byte string</returns>
    public static byte[] GetASCIIBytes(string s) => Encoding.ASCII.GetBytes(s);

    /// <summary>
    /// Calls <see cref="Encoding.ASCII.GetString(byte[])"/>
    /// </summary>
    /// <param name="bytes">The byte array to decode</param>
    /// <returns>The ASCII decoded string</returns>
    public static string GetASCIIString(byte[] bytes) => ASCII.GetString(bytes);

    /// <summary>
    /// Calls <see cref="Encoding.UTF7.GetBytes(s)"/>
    /// </summary>
    /// <param name="s">The string to encode</param>
    /// <returns>The UTF7 encoded byte string</returns>
#if NET6_0_OR_GREATER
    [Obsolete("Encoding.UTF7' is obsolete: The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#else
    [Obsolete(
        "Encoding.UTF7' is obsolete: The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead."
    )]
#endif
    public static byte[] GetUTF7Bytes(string s) => UTF7.GetBytes(s);

    /// <summary>
    /// Calls <see cref="Encoding.UTF7.GetString(byte[])"/>
    /// </summary>
    /// <param name="bytes">The byte array to decode</param>
    /// <returns>The UTF7 decoded string</returns>
#if NET6_0_OR_GREATER
    [Obsolete("Encoding.UTF7' is obsolete: The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead.", DiagnosticId = "SYSLIB0001", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
#else
    [Obsolete(
        "Encoding.UTF7' is obsolete: The UTF-7 encoding is insecure and should not be used. Consider using UTF-8 instead."
    )]
#endif
    public static string GetUTF7String(byte[] bytes) => UTF7.GetString(bytes);
#endif

    /// <summary>
    /// Returns <inheritdoc cref="ToHexString" path="/returns" />
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>the <paramref name="bytes" /> as a hex string</returns>
    public static string ToHexString(this byte[] bytes) =>
        Join("", bytes.Select(b => b.ToString("X2")));

    /// <summary>
    /// Returns <inheritdoc cref="FromHexString" path="/returns" />
    /// </summary>
    /// <param name="s"></param>
    /// <returns>the <paramref name="s" /> hex string as a byte array</returns>
    public static byte[] FromHexString(this string s)
    {
#if NET5_0_OR_GREATER
        return global::System.Convert.FromHexString(s);
#else
        var buff = new byte[s.Length * 2];
        for (var i = 0; i < s.Length; i++)
        {
            buff[i / 2] = ToHexBytes(s[i]);
        }
        return buff;
#endif
    }

    private static byte ToHexBytes(char c)
    {
        return c switch
        {
            '0' => 0x0,
            '1' => 0x1,
            '2' => 0x2,
            '3' => 0x3,
            '4' => 0x4,
            '5' => 0x5,
            '6' => 0x6,
            '7' => 0x7,
            '8' => 0x8,
            '9' => 0x9,
            'a' => 0x10,
            'b' => 0x11,
            'c' => 0x12,
            'd' => 0x13,
            'e' => 0x14,
            'f' => 0x15,
            _ => throw new ArgumentException("Argument must be a hexadecimal digit.", nameof(c))
        };
    }
}