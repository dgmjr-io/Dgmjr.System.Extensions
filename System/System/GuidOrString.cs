/*
 * GuidOrString.cs
 *     Created: 2024-18-08T02:18:13-05:00
 *    Modified: 2024-29-19T13:29:07-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

public class GuidOrString : TOrString<GuidOrString, guid>
{
    public GuidOrString(string str)
        : base(str) { }

    public GuidOrString(guid guid)
        : base(guid) { }
}
