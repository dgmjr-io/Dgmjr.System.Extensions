namespace System.Text.Json;

public class JsonUnixTimeConverter : JsonConverter<datetime>
{
    public override datetime Read(ref Utf8JsonReader reader, type typeToConvert, Jso options) =>
        reader.TokenType != JTokenType.Number
            ? throw new JException()
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

public class JsonUnixTimeMillisecondsConverter : JsonConverter<datetime>
{
    public override datetime Read(ref Utf8JsonReader reader, type typeToConvert, Jso options) =>
        reader.TokenType != JTokenType.Number
            ? throw new JException()
            : DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime;

    public override void Write(Utf8JsonWriter writer, datetime value, Jso options) =>
        writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
}

public class JsonUnixTimeMillisecondsDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(
        ref Utf8JsonReader reader,
        type typeToConvert,
        Jso options
    ) =>
        reader.TokenType != JTokenType.Number
            ? throw new JException()
            : DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, Jso options) =>
        writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
}

public class JsonConverterAttribute<T> : JConverterAttribute
{
    public JsonConverterAttribute()
        : base(typeof(T)) { }
}
