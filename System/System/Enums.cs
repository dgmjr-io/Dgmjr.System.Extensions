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
