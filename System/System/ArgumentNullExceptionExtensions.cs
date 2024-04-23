namespace System;

using System.Runtime.CompilerServices;

public static class ArgumentNullExceptionExtensions
{
    public static void ThrowIfNull(
        this object? value,
        [CallerArgumentExpression("value")] string? paramName = null
    )
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}

public static class ArgumentExceptionExtensions
{
    public static void ThrowIfNullOrEmpty(
        this object? value,
        [CallerArgumentExpression("value")] string? paramName = null
    )
    {
        if (IsNullOrEmpty(value.ToString()))
        {
            throw new ArgumentException(paramName);
        }
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
