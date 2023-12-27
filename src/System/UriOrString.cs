namespace System;

using OneOf;

public class UriOrString : OneOfBase<Uri, string>
{
    public UriOrString(Uri uri)
        : base(uri) { }

    public UriOrString(string str)
        : base(str) { }

    public static implicit operator UriOrString(Uri uri) => new(uri);

    public static implicit operator UriOrString(string str) => new(str);

    public static implicit operator Uri(UriOrString uriOrString) =>
        uriOrString.IsT0 ? uriOrString.AsT0 : new(uriOrString.AsT1);

    public override string ToString()
    {
        return ((Uri)this).ToString();
    }

    public static explicit operator string(UriOrString uriOrString) =>
        uriOrString.IsT1 ? uriOrString.AsT1 : uriOrString.AsT0.ToString();
}
