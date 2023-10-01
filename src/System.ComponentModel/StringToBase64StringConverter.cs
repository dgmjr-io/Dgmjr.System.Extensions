/*
 * StringToBase64StringConverter.cs
 *
 *   Created: 2023-02-24-03:24:13
 *   Modified: 2023-02-24-03:24:15
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright ©2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.ComponentModel;

using System.Text;

/// <summary>
/// Defines a value converter from <see cref="string"/> to Base64 <see cref="string"/> with an optional Encoding parameter.
/// </summary>
public partial class StringToBase64StringValueConverter
#if NETSTANDARD1
    : System.ComponentModel.TypeConverter
#endif
{
    /// <summary>
    /// Converts the <paramref name="value">value</paramref> to the Base64 <see cref="string"/>.
    /// </summary>
    /// <param name="value">
    /// The value to convert.
    /// </param>
    /// <param name="parameter">
    /// The optional <see cref="Encoding"/> parameter used to help with conversion.
    /// </param>
    /// <returns>
    /// The converted Base64 <see cref="string"/> object.
    /// </returns>
    public string Convert(string value, object? parameter = default)
    {
        return System.Convert.ToBase64String(
            (parameter as Encoding ?? Encoding.UTF8).GetBytes(value)
        );
    }

    /// <summary>
    /// Converts the Base64 <paramref name="value">value</paramref> back to the original <see cref="string"/> value.
    /// </summary>
    /// <param name="value">
    /// The value to convert.
    /// </param>
    /// <param name="parameter">
    /// The optional <see cref="Encoding"/> parameter used to help with conversion.
    /// </param>
    /// <returns>
    /// The converted <see cref="string"/> object.
    /// </returns>
    public string ConvertBack(string value, object? parameter = default)
    {
        return (parameter as Encoding ?? Encoding.UTF8).GetString(
            System.Convert.FromBase64String(value)
        );
    }
}
