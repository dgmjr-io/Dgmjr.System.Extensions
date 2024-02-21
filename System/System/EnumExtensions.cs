namespace System;

public static class EnumExtensions
{
    public static int ToInt32<T>(this T value)
        where T : Enum =>
        ConvertTo<T, int>(value, out var result)
            ? result
            : throw new InvalidCastException("Could not convert to int32");

    public static long ToInt64<T>(this T value)
        where T : Enum
        => value.ConvertTo<T, long>(out var result)
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
}
