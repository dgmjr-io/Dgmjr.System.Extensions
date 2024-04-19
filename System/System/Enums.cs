/*
 * Enums.cs
 *     Created: 2024-48-26T01:48:06-05:00
 *    Modified: 2024-27-19T13:27:55-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

public static class Enums
{
    public static T[] GetValues<T>()
        where T : Enum
    {
        return Enum.GetValues(typeof(T)).OfType<T>().ToArray();
    }

    public static T Parse<T>(string s)
        where T : Enum
    {
        return Enum.Parse(typeof(T), s, true).To<T>();
    }
}
