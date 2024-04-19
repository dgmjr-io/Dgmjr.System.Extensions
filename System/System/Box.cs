/*
 * Box.cs
 *     Created: 2024-01-16T18:01:13-04:00
 *    Modified: 2024-31-19T13:31:54-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

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
