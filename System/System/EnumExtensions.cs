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
}
