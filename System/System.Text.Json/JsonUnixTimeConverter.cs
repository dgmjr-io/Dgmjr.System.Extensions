namespace System.Text.Json;

public class JsonUnixTimeConverter : JsonConverter<datetime>
{
    public override datetime Read(ref Utf8JsonReader reader, type typeToConvert, Jso options) =>
        reader.TokenType != JTokenType.Number
            ? throw new JsonException()
            : DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).DateTime;

    public override void Write(Utf8JsonWriter writer, datetime value, Jso options) =>
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeSeconds());
}

public class JsonUnixTimeDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        type typeToConvert,
        Jso options
    ) =>
        reader.TokenType != JTokenType.Number
            ? throw new JsonException()
            : DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, Jso options) =>
        writer.WriteNumberValue(value.ToUnixTimeSeconds());
}
