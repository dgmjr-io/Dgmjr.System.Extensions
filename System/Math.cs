/*
 *
 * Math.cs
 *
 *   Created: 2022-11-12-07:23:53
 *   Modified: 2022-11-12-07:25:59
 *
 *   Author: David G. Mooore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 *
 */

namespace System;
using static System.Math;

public static class MathExtensions
{
    public const double π = PI;

    /// <summary>
    /// Converts a value from degrees to radians. This is equivalent to the C# function Math. ToRadians ( double )
    /// </summary>
    /// <param name="degrees">The value in degrees</param>
    /// <returns>The value in radians as a double value with the same units as degrees ( 0 to 360 ). If degrees is greater than 360 the value is returned</returns>
    public static double ToRadians(this double degrees) => degrees * (π / 180.0);

    /// <summary>
    /// Converts an angle in degrees to degrees. This is equivalent to the Cosine mathematical expression
    /// </summary>
    /// <param name="radians">The value in radians</param>
    /// <returns>The angle in degrees as a double value if the angle is valid or NaN if the angle is out of</returns>
    public static double ToDegrees(this double radians) => radians * (180.0 / π);

    /// <summary>
    /// Determines whether a double - precision floating - point number is between a specified range. A return value indicates whether the value is inclusively between the min and max values.
    /// </summary>
    /// <param name="value">The value to determine if it's between <paramref name="min"/> and <paramref name="max"/>.</param>
    /// <param name="min">The minimum value ( inclusive ). If value is greater than min this method returns false.</param>
    /// <param name="max">The maximum value ( inclusive ). If value is greater than max this method returns false.</param>
    /// <returns>True if value is inclusively between min and max ; otherwise false.</returns>
    /// <example>For value == 0.6, if min is 0. 5 and max is 1 then true is returned</example>
    public static bool IsBetween(this double value, double min, double max) =>
        value >= min && value <= max;

    /// <summary>
    /// Determines whether the specified value is between the min and max values. A value is between 0 and 1 inclusively.
    /// </summary>
    /// <param name="value">The value to determine if it's between <paramref name="min"/> and <paramref name="max"/>.</param>
    /// <param name="min">The minimum value. Must be greater than or equal to max.</param>
    /// <param name="max">The maximum value. Must be greater than or equal to min.</param>
    /// <returns>True if value is inclusively between min and max ; otherwise false.</returns>
    /// <example>For value == 1, if min is 0 and max is 1 then true is returned</example>
    public static bool IsBetween(this int value, int min, int max) => value >= min && value <= max;

    /// <summary>
    /// Determines whether a 64 - bit signed integer is inclusively between two values. A return of true indicates that the value is inclusively between the two values ; otherwise false.
    /// </summary>
    /// <param name="value">The value to determine if it's between <paramref name="min"/> and <paramref name="max"/>.</param>
    /// <param name="min">The minimum value. Must be greater than or equal to max.</param>
    /// <param name="max">The maximum value. Must be greater than or equal to min.</param>
    /// <returns>True if value is inclusively between min and max ; otherwise false. For example 0 would return true</returns>
    public static bool IsBetween(this long value, long min, long max) =>
        value >= min && value <= max;

    /// <summary>
    /// Determines whether or not a single - precision floating - point number is inclusively between two specified values.
    /// </summary>
    /// <param name="value">The value to determine if it's between <paramref name="min"/> and <paramref name="max"/>.</param>
    /// <param name="min">The minimum value of the range. If value is greater than min this method returns false.</param>
    /// <param name="max">The maximum value of the range. If value is greater than max this method returns false.</param>
    /// <returns>True if value is inclusively between min and max ; otherwise false.</returns>
    public static bool IsBetween(this float value, float min, float max) =>
        value >= min && value <= max;

    public static bool IsBetween(this IComparable value, IComparable min, IComparable max) =>
        value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
}
