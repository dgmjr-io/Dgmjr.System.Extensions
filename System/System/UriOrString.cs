namespace System;

using OneOf;

public class UriOrString(string str) : OneOfBase<Uri, string>(str)
{
    public UriOrString(Uri uri)
        : this(uri.ToString()) { }

public static implicit operator UriOrString(Uri uri) => new(uri);

public static implicit operator UriOrString(string str) => new(str);

public static implicit operator Uri(UriOrString uriOrString) =>
    uriOrIsT0 ? uriOrString.AsT0 : new(uriOrString.AsT1);

public override string ToString()
{
    return ((Uri)this).ToString();
}

public static explicit operator string(UriOrString uriOrString) =>
    uriOrIsT1 ? uriOrString.AsT1 : uriOrString.AsT0.ToString();
}
