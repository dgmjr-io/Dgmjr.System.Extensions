namespace System;

public static class TypeExtensions
{
    public static string GetDisplayName(this Type type)
    {
        return type.IsGenericType || type.IsGenericTypeDefinition
            ? $"{type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(", ", type.GetGenericArguments().Select(GetDisplayName))}>"
            : type.Name;
    }
}
