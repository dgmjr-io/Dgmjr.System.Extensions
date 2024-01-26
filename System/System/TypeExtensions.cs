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
