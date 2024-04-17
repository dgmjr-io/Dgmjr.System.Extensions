using System.Xml;

namespace System.Text.Json;

public class Iso8601TimeSpanConverter : JsonConverter<duration>
{
    public override duration Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        if (reader.TokenType is not JTokenType.String)
        {
            throw new JException("Error reading duration. Unexpected token: {reader.TokenType}");
        }
        return XmlConvert.ToTimeSpan(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, duration value, Jso options)
    {
        writer.WriteStringValue(XmlConvert.ToString(value));
    }
}
