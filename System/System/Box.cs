namespace System;

public class Box<T>
    where T : notnull
{
    public T Value { get; set; }

    public Box() { }

    public Box(T value) => Value = value;

    public static implicit operator Box<T>(T value) => new(value);

    public static implicit operator T(Box<T> value) => value.Value;
}
