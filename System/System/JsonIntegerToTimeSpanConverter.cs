/*
 * JsonIntegerToTimeSpanConverter.cs
 *     Created: 2023-23-22T07:23:06-05:00
 *    Modified: 2024-29-19T13:29:34-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

using System.Text.Json.Serialization;
using System.Text.Json;
using Jso = System.Text.Json.JsonSerializerOptions;

public class JsonIntegerToTimeSpanConverter : JsonConverter<duration>
{
    public override duration Read(ref Utf8JsonReader reader, type typeToConvert, Jso options) =>
        reader.TokenType != JTokenType.Number
            ? throw new JsonException()
            : duration.FromSeconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, duration value, Jso options) =>
        writer.WriteNumberValue(value.TotalSeconds);
}
