/*
 * EnumExtensions.cs
 *     Created: 2024-19-20T15:19:44-05:00
 *    Modified: 2024-27-19T13:27:43-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

using System.ComponentModel.DataAnnotations;

public static class EnumExtensions
{
    public static int ToInt32<T>(this T value)
        where T : Enum =>
        ConvertTo<T, int>(value, out var result)
            ? result
            : throw new InvalidCastException("Could not convert to int32");

    public static long ToInt64<T>(this T value)
        where T : Enum =>
        value.ConvertTo<T, long>(out var result)
            ? result
            : throw new InvalidCastException("Could not convert to int64");

    public static bool ConvertTo<T, U>(this T value, out U result)
        where T : Enum
    {
        try
        {
            var uValue = Convert.ChangeType(value, typeof(U));
            if (uValue is U u)
            {
                result = u;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        catch
        {
            result = default;
            return false;
        }
    }

    public static FieldInfo? GetFieldInfo<T>(this T e)
        where T : Enum => e.GetType().GetField(e.ToString());

    public static TAttribute? GetCustomAttribute<TAttribute>(this Enum e)
        where TAttribute : Attribute => e.GetFieldInfo()?.GetCustomAttribute<TAttribute>();

    public static string GetShortName<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return (
            attribute?.ShortName?.IsPresent() == true ? attribute.GetShortName() : e.ToString()
        )!;
    }

    public static string GetDisplayName<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return (attribute?.ShortName?.IsPresent() == true ? attribute.GetName() : e.ToString())!;
    }

    public static int GetOrder<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return attribute?.GetOrder() ?? 0;
    }

    public static string GetDescription<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return (
            attribute?.Description?.IsPresent() == true ? attribute.GetDescription() : e.ToString()
        )!;
    }

    public static string GetCategory<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return (
            attribute?.GetGroupName()?.IsPresent() == true ? attribute.GetGroupName() : e.ToString()
        )!;
    }

    public static string[] GetSynonyms<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<SynonymsAttribute>();
        return (attribute?.Value ?? Empty<string>())!;
    }

    public static string GetName<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<DisplayAttribute>();
        return (attribute?.GetName() ?? e.ToString())!;
    }

    public static guid? GetGuid<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<GuidAttribute>();
        return attribute?.Value;
    }

    public static Uri? GetUri<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<UriAttribute>();
        return attribute?.Value;
    }

    public static System.Runtime.Serialization.EnumMemberAttribute? GetEnumMember<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetCustomAttribute<System.Runtime.Serialization.EnumMemberAttribute>();
        return attribute;
    }

    public static string GetEnumMemberValue<T>(this T e)
        where T : Enum
    {
        var attribute = e.GetEnumMember();
        return attribute?.Value ?? e.ToString();
    }

    public static string[] GetStringValues<T>(this T e)
        where T : Enum
    {
        return new[]
        {
            e.ToString(),
            e.GetName(),
            e.GetShortName(),
            e.GetDisplayName(),
            e.GetUri().ToString(),
            e.GetGuid().ToString()
        }
            .Concat(e.GetSynonyms())
            .Distinct()
            .ToArray();
    }
}
