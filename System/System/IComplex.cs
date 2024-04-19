/*
 * IComplex.cs
 *     Created: 2023-09-21T20:09:26-04:00
 *    Modified: 2024-29-19T13:29:29-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Numerics;

// [GenerateInterface(typeof(System.Numerics.Complex))]
public partial interface IComplex : IComparable
{
    double Imaginary { get; }
    double Magnitude { get; }
    double Phase { get; }
    double Real { get; }
    bool Equals(Complex value);
    string ToString(IFormatProvider provider);
    string ToString(string format);
    string ToString(string format, IFormatProvider provider);
}
