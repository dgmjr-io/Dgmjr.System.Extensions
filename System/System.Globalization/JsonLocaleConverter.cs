namespace System.Globalization;

using System.Text.Json.Serialization;
using System.Text.Json;
using Jso = System.Text.Json.JsonSerializerOptions;

public class JsonLocaleConverter
    : System.Text.Json.Serialization.JsonConverter<System.Globalization.CultureInfo>
{
    public override System.Globalization.CultureInfo Read(
        ref Utf8JsonReader reader,
        type typeToConvert,
        Jso options
    )
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            try
            {
                return System.Globalization.CultureInfo.CreateSpecificCulture(reader.GetString());
            }
            catch
            {
                return System.Globalization.CultureInfo.InvariantCulture;
            }
        }
        else
        {
            return System.Globalization.CultureInfo.InvariantCulture;
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        System.Globalization.CultureInfo value,
        Jso options
    )
    {
        writer.WriteStringValue(value.Name);
    }
}
