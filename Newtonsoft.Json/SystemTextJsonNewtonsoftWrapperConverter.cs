using Newtonsoft.Json;

using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace System.Text.Json;

public class SystemTextJsonNewtonsoftJsonWrapperConverter<T, TNewtonsoftConverter>(
    TNewtonsoftConverter newtonsoftConverter
) : Serialization.JsonConverter<T>
    where TNewtonsoftConverter : Newtonsoft.Json.JsonConverter<T>, new()
{
    public SystemTextJsonNewtonsoftJsonWrapperConverter()
        : this(new TNewtonsoftConverter()) { }

    protected virtual TNewtonsoftConverter NewtonsoftConverter => newtonsoftConverter;

    public override T? Read(ref Utf8JsonReader reader, type typeToConvert, Jso options)
    {
        var json = reader.GetString();
        var newtonsoftObject = JsonConvert.DeserializeObject<T>(json, NewtonsoftConverter);
        return newtonsoftObject;
    }

    public override void Write(Utf8JsonWriter writer, T value, Jso options)
    {
        var json = JsonConvert.SerializeObject(value, NewtonsoftConverter);
        writer.WriteStringValue(json);
    }
}

public class SystemTextJsonNewtonsoftJsonWrapperConverterAttribute<T, TNewtonsoftConverter>
    : JsonConverterAttribute
    where TNewtonsoftConverter : Newtonsoft.Json.JsonConverter<T>, new()
{
    public SystemTextJsonNewtonsoftJsonWrapperConverterAttribute()
        : base(typeof(SystemTextJsonNewtonsoftJsonWrapperConverter<T, TNewtonsoftConverter>)) { }
}
