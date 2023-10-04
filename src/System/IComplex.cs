/*
 * IComplex.cs
 *
 *   Created: 2023-09-21-08:09:26
 *   Modified: 2023-10-02-04:54:32
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Numerics;

// [GenerateInterface(typeof(System.Numerics.Complex))]
public partial interface IComplex : IComparable
{
    double Imaginary { get; }
    double Magnitude { get; }
    double Phase { get; }
    double Real { get; }
    bool Equals(global::System.Numerics.Complex value);
    string ToString(global::System.IFormatProvider provider);
    string ToString(string format);
    string ToString(string format, global::System.IFormatProvider provider);
}
