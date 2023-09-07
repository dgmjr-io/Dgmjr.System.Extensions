/*
 * Randoms.cs
 *
 *   Created: 2022-10-23-11:42:05
 *   Modified: 2022-12-02-11:19:03
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System;
using System.Linq;
using System.Xml.Schema;

namespace System;
using static System.BitConverter;
using static System.Convert;
using static System.Math;

/// <summary>
/// Extensions to the <see cref="Random"/> class
/// </summary>
public sealed class Randoms : Random
{
    /// <summary>
    /// Generates <inheritdoc cref="NextGuid" path="/returns" />
    /// </summary>
    /// <returns>a new GUID (UUID)</returns>
    public static Guid NextGuid() => Guid.NewGuid();

    /// <summary>
    /// Generates <inheritdoc cref="Next16Bytes" path="/returns" />
    /// </summary>
    /// <returns>an array of 16 Next bytes</returns>

    public static byte[] Next16Bytes() => NextGuid().ToByteArray();

    /// <summary>
    /// Generates <inheritdoc cref="NextBytes" path="/returns" />
    /// </summary>
    /// <returns>an array of <paramref name="n"/> new bytes</returns>
    /// <param name="n">The number of bytes to generate</param>
    public static byte[] NextBytes(int n) =>
        Enumerable.Range(0, n / 16)
                    .Select(_ => Next16Bytes())
                    .SelectMany(_ => _)
                    .Concat(Next16Bytes().Take(n % 16)).ToArray();

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt8()" path="/returns"/>
    /// </summary>
    /// <returns>a new unsigned byte value</returns>
    public static byte NextUInt8() => NextGuid().ToByteArray().First();

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt8(byte,byte)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextUInt8()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static byte NextUInt8(byte from = byte.MinValue, byte to = byte.MaxValue) => ToByte(from + (NextUInt8() % (to - from)));

    /// <summary>
    /// Generates <inheritdoc cref="NextInt8()" path="/returns"/>
    /// </summary>
    /// <returns>a new signed 8-bit integer value</returns>
    public static sbyte NextInt8() => (sbyte)NextGuid().ToByteArray().First();

    /// <summary>
    /// Generates <inheritdoc cref="NextInt8(sbyte,sbyte)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextInt8()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static sbyte NextInt8(sbyte from = sbyte.MinValue, sbyte to = sbyte.MaxValue) => ToSByte(from + (NextInt8() % (to - from)));
    #region Int16

    /// <summary>
    /// Generates <inheritdoc cref="NextInt16()" path="/returns"/>
    /// </summary>
    /// <returns>a new signed <see langword="short"/> value</returns>
    public static short NextInt16() => ToInt16(NextGuid().ToByteArray(), 0);

    /// <summary>
    /// Generates <inheritdoc cref="NextInt16( short, short)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextInt16()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static short NextInt16(short from = short.MinValue, short to = short.MaxValue) => Convert.ToInt16(Abs(from + (NextInt16() % (to - from))));

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt16()" path="/returns"/>
    /// </summary>
    /// <returns>a new unsigned 16-bit integer value</returns>
    public static short NextUInt16() => ToInt16(NextGuid().ToByteArray(), 0);

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt16(ushort,ushort)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextUInt16()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static ushort NextUInt16(ushort from = ushort.MinValue, ushort to = ushort.MaxValue) => Convert.ToUInt16(Abs(from + (NextUInt16() % (to - from))));
    #endregion

    #region Int32

    /// <summary>
    /// Generates <inheritdoc cref="NextInt32()" path="/returns"/>
    /// </summary>
    /// <returns>a new signed int value</returns>
    public static int NextInt32() => BitConverter.ToInt32(NextGuid().ToByteArray(), 0);

    /// <summary>
    /// Generates <inheritdoc cref="NextInt32( int, int)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextInt32()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static int NextInt32(int from = int.MinValue, int to = int.MaxValue) => Abs(from + (NextInt32() % (to - from)));

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt32()" path="/returns"/>
    /// </summary>
    /// <returns>a new unsigned 32-bit integer value</returns>
    public static int NextUInt32() => ToInt32(NextGuid().ToByteArray(), 0);

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt32(UInt32,UInt32)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextUInt32()" path="/returns"/> in the range
    ///     [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static UInt32 NextUInt32(UInt32 from = UInt32.MinValue, UInt32 to = UInt32.MaxValue) => Convert.ToUInt32(Abs(from + (NextUInt32() % (to - from))));
    #endregion

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt64(long, long)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextUInt64()" path="/returns"/> in the range
    ///     [<paramref name="from"/>,<paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static long NextUInt64(long from = long.MinValue, long to = long.MaxValue) => from + (NextInt64() % (to - from));

    /// <summary>
    /// Generates <inheritdoc cref="NextUInt64()" path="/returns"/>
    /// </summary>
    /// <returns>a new unsigned 64-bit integer value</returns>
    public static UInt64 NextUInt64() => ToUInt64(NextGuid().ToByteArray(), 0);


    /// <summary>
    /// Generates <inheritdoc cref="NextUInt64()" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextUInt64()" path="/returns"/></returns>
    public static
#if NET6_0_OR_GREATER
    new
#endif
    long NextInt64() => ToInt64(NextGuid().ToByteArray(), 0);

    /// <summary>
    /// Generates <inheritdoc cref="NextInt64(long, long)" path="/returns"/>
    /// </summary>
    /// <returns><inheritdoc cref="NextInt64()" path="/returns"/> in the range
    ///     [<paramref name="from"/>,<paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next
    ///     number</param>
    /// <param name="to">The upper bound of the range to generate the Next
    ///     number</param>
    public static
#if NET6_0_OR_GREATER
    new
#endif
    long NextInt64(long from = long.MinValue, long to = long.MaxValue) => from + (NextInt64() % (to - from));


    /// <summary>
    /// Generates <inheritdoc cref="NextBoolean" path="/returns" />
    /// </summary>
    /// <param name="pTrue"></param>
    /// <returns>a new Boolean value with the probability of being <see langword="true" /> = <paramref name="pTrue" /></returns>
    public static bool NextBoolean(double pTrue = 0.5) => NextInt32() % Convert.ToInt32(1 / pTrue) == 0;

#if NET7_0_OR_GREATER
    /// <summary>Generates <inheritdoc cref="NextInt128(Int128?, Int128?)" path="/returns" /></summary>
    /// <returns><inheritdoc cref="NextInt128()" path="/returns"/> in the range [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next number</param>
    /// <param name="to">The upper bound of the range to generate the Next number</param>
    public static vlong NextInt128(vlong? from = null, vlong? to = null) => (from ?? vlong.MinValue) + (NextInt128() % (to ?? vlong.MaxValue - from ?? vlong.MinValue));

    /// <summary>Generates <inheritdoc cref="NextInt128()" path="/returns"/></summary>
    /// <returns>a new <see cref="Int128" /></returns>
    public static vlong NextInt128() => new vlong(NextUInt64(), NextUInt64());


    /// <summary>Generates <inheritdoc cref="NextUInt128(UInt128?,UInt128?)" path="/returns" /></summary>
    /// <returns><inheritdoc cref="NextUInt128()" path="/returns"/> in the range [<paramref name="from"/>, <paramref name="to"/>]</returns>
    /// <param name="from">The lower bound of the range to generate the Next number</param>
    /// <param name="to">The upper bound of the range to generate the Next number</param>
    public static uvlong NextUInt128(uvlong? from = null, uvlong? to = null) => (from ?? uvlong.MinValue) + (NextUInt128() % (to ?? uvlong.MaxValue - from ?? uvlong.MinValue));

    /// <summary>Generates <inheritdoc cref="NextUInt128()" path="/returns"/></summary>
    /// <returns>a new <see cref="UInt128" /></returns>
    public static uvlong NextUInt128() => new uvlong(NextUInt64(), NextUInt64());
#endif
}
