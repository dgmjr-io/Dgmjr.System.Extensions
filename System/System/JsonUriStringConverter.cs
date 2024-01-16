namespace System;

public class JsonUriStringConverter : System.Text.Json.Serialization.JsonConverter<Uri>
{
    public override bool CanConvert(type typeToConvert) => typeToConvert == typeof(Uri);

    public override Uri? Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        try
        {
            return new(reader.GetString());
        }
        catch
        {
            return default;
        }
    }

    public override void Write(Utf8JsonWriter writer, Uri value, Jso options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class JsonUriOrStringConverter : System.Text.Json.Serialization.JsonConverter<UriOrString>
{
    public override bool CanConvert(type typeToConvert) => typeToConvert == typeof(UriOrString);

    public override UriOrString Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        return new(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, UriOrString value, Jso options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class JsonUriStringAttribute : JConverterAttribute
{
    public override JConverter CreateConverter(type typeToConvert)
    {
        return new JsonUriStringConverter();
    }
}

public class JsonUriOrStringStringAttribute : JConverterAttribute
{
    public override JConverter CreateConverter(type typeToConvert)
    {
        return new JsonUriStringConverter();
    }
}
