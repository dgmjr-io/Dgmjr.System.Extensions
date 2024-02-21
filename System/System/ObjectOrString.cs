namespace System;

using OneOf;

public abstract class TOrString<TSelf, T> where TSelf : TOrString<TSelf, T>
{
    protected TOrString(string str)
    {
        Value = str;
    }

    protected TOrString(T t)
    {
        Value = t;
    }

    private object Value { get; }

    private T? AsT => (T?)Value;
    private string? String => Value as string;

    public bool IsT => AsT is not null;
    public bool IsString => String != null;


    public static implicit operator TOrString<TSelf, T>(T t) => (Activator.CreateInstance(typeof(TSelf), t) as TSelf)!;

    public static implicit operator TOrString<TSelf, T>(string str) => (Activator.CreateInstance(typeof(TSelf), str) as TSelf)!;

    public static implicit operator T(TOrString<TSelf, T> tOrString) =>
        tOrString.IsT ? tOrString.AsT! : (Activator.CreateInstance(typeof(TSelf), tOrString) as TSelf)!;

    public static explicit operator string(TOrString<TSelf, T> tOrString) => tOrString.ToString();

    public override string ToString() => IsString ? String! : AsT!.ToString();
}
