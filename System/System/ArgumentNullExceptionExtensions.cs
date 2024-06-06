namespace System;

using System.Runtime.CompilerServices;

public static class ArgumentNullExceptionExtensions
{
    public static T ThrowIfNull<T>(
        this T? value,
        [CallerArgumentExpression("value")] string? paramName = null
    )
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
        else
            return value;
    }
}

public static class ArgumentExceptionExtensions
{
    public static T ThrowIfNullOrEmpty<T>(
        this T? value,
        [CallerArgumentExpression("value")] string? paramName = null
    )
    {
        if (IsNullOrEmpty(value?.ToString()))
        {
            throw new ArgumentException(paramName);
        }
        else
            return value;
    }
}

public static class ThrowHelpers
{
    public static void ThrowIf(
        bool condition,
        [CallerArgumentExpression(nameof(condition))] string? paramName = null
    )
    {
        if (condition)
        {
            throw new InvalidOperationException($"The condition {paramName} was true.");
        }
    }
}
