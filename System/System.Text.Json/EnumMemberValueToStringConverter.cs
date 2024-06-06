/*
 * EnumValueToStringConverter.cs
 *     Created: 2024-05-05T20:43:59-04:00
 *    Modified: 2024-05-05T20:43:59-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Text.Json;

namespace System.Text.Json;

public class EnumMemberValueToStringConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        if (reader.TokenType != JTokenType.String)
        {
            throw new JException($"Expected a string but got {reader.TokenType}");
        }

        var enumString = reader.GetString() ?? throw new JException("Expected a non-null string");

        foreach (var value in Enums.GetValues<TEnum>())
        {
            if (value.GetEnumMemberValue() == enumString)
            {
                return value;
            }
        }

        throw new JException($"Unable to convert '{enumString}' to {typeof(TEnum).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, Jso options)
    {
        writer.WriteStringValue(value.GetEnumMemberValue());
    }
}
