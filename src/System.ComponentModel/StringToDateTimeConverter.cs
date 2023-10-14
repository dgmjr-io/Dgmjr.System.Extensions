/*
 * StringToDateTimeConverter.cs
 *
 *   Created: 2023-02-24-03:12:02
 *   Modified: 2023-02-24-03:12:03
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
namespace System.ComponentModel;

using System.Globalization;

/// <summary>
/// Defines a value converter from <see cref="DateTime"/> to <see cref="string"/> with an optional format string.
/// </summary>
public partial class DateTimeToStringValueConverter
#if NETSTANDARD1
    : System.ComponentModel.TypeConverter
#endif
{
    /// <summary>
    /// Converts the <paramref name="value">value</paramref> to the <see cref="string"/> type.
    /// </summary>
    /// <param name="value">
    /// The value to convert.
    /// </param>
    /// <param name="parameter">
    /// The optional parameter used to help with conversion.
    /// </param>
    /// <returns>
    /// The converted <see cref="string"/> object.
    /// </returns>
    public string Convert(DateTime value, object? parameter = default)
    {
        string? format = parameter?.ToString();
        return !IsNullOrWhiteSpace(format)
            ? value.ToString(format, CultureInfo.InvariantCulture)
            : value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the <paramref name="value">value</paramref> back to the <see cref="DateTime"/> type.
    /// </summary>
    /// <param name="value">
    /// The value to convert.
    /// </param>
    /// <param name="parameter">
    /// The optional parameter used to help with conversion.
    /// </param>
    /// <returns>
    /// The converted <see cref="DateTime"/> object.
    /// </returns>
    public DateTime ConvertBack(string value, object? parameter = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DateTime.MinValue;
        }

        bool parsed = DateTime.TryParse(value, out DateTime dateTime);
        return parsed ? dateTime : DateTime.MinValue;
    }
}
