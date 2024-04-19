/*
 * TypeExtensions.cs
 *     Created: 2024-31-02T00:31:15-05:00
 *    Modified: 2024-30-19T13:30:55-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

using System.Text.RegularExpressions;

public static class TypeExtensions
{
    public static string GetDisplayName(this Type type)
    {
        return Regex.Replace(type.GetDisplayNameInternal(), @"[`\-$]", "_");
    }

    internal static string GetDisplayNameInternal(this Type type)
    {
        try
        {
            return type.IsGenericType || type.IsGenericTypeDefinition
                ? $"{type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(", ", type.GetGenericArguments().Select(GetDisplayName))}>"
                : type.Name;
        }
        catch
        {
            return type.FullName!;
        }
    }
}
