/*
 * UriOrString.cs
 *     Created: 2023-46-22T02:46:39-05:00
 *    Modified: 2024-31-19T13:31:46-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

public class UriOrString
{
    public UriOrString(string str)
    {
        Value = str;
    }

    public UriOrString(Uri uri)
    {
        Value = uri;
    }

    private object Value { get; }

    private Uri? Uri => Value as Uri;
    private string? String => Value as string;

    public bool IsUri => Uri != null;
    public bool IsString => String != null;

    public static implicit operator UriOrString(Uri uri) => new(uri);

    public static implicit operator UriOrString(string str) => new(str);

    public static implicit operator Uri(UriOrString uriOrString) =>
        uriOrString.IsUri ? uriOrString.Uri! : new(uriOrString.String);

    public static explicit operator string(UriOrString uriOrString) => uriOrString.ToString();

    public override string ToString() => IsString ? String! : Uri!.ToString();
}
