/*
 * DictionaryStringJsonConverter.cs
 *
 *   Created: 2024-56-20T05:56:46-05:00
 *   Modified: 2024-56-20T05:56:46-05:00
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2024 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System.Text.Json;

/// <summary>
///     Provides the ability to convert JSON key/value pairs to a <type name="IDictionary" />.
/// </summary>
public class StringDictionaryJsonConverter : JsonConverter<IStringDictionary>
{
    public override IStringDictionary Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        Jso options
    )
    {
        var dictionary = new StringDictionary();

        while (reader.Read())
        {
            if (reader.TokenType == JTokenType.EndObject)
                return dictionary;

            if (reader.TokenType != JTokenType.PropertyName)
                throw new JsonException("Did not encounter PropertyName");

            var propertyName = reader.GetString();

            if (IsNullOrWhiteSpace(propertyName))
                throw new JsonException("Failed to get property name");

            reader.Read();

            var propertyValue = ExtractString(ref reader);

            if (!IsNullOrWhiteSpace(propertyValue))
                dictionary.Add(propertyName, propertyValue);
        }

        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, IStringDictionary value, Jso options)
    {
        Serialize(writer, value, options);
    }

    private static string? ExtractString(ref Utf8JsonReader reader)
    {
        switch (reader.TokenType)
        {
            case JTokenType.String:
                return reader.GetString()!;

            case JTokenType.Number:
                if (reader.TryGetInt64(out var result))
                    return result.ToString();
                else if (reader.TryGetDecimal(out var d))
                    return d.ToString("N");
                throw new JsonException("Unable to convert to string");

            case JTokenType.True:
                return $"{true}";

            case JTokenType.False:
                return $"{false}";

            case JTokenType.Null:
                return null;

            default:
                throw new JsonException($"'{reader.TokenType}' is not supported");
        }
    }
}
