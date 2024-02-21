namespace System;

public class GuidOrString : TOrString<GuidOrString, guid>
{
    public GuidOrString(string str) : base(str) { }
    public GuidOrString(guid guid) : base(guid) { }
}
