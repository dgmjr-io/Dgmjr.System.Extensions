namespace System;

using System.Text.Json.Serialization;
using System.Text.Json;
using Jso = System.Text.Json.JsonSerializerOptions;

public class JsonIntegerToTimeSpanConverter : JsonConverter<duration>
{
    public override duration Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
        => reader.TokenType != JTokenType.Number ? throw new JsonException() : duration.FromSeconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, duration value, Jso options)
        => writer.WriteNumberValue(value.TotalSeconds);
}
