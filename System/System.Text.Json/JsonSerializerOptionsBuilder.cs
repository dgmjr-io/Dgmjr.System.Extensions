namespace System.Text.Json;

using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization.Metadata;

public class JsonSerializerOptionsBuilder : IJsonSerializerOptions
{
    public IJsonTypeInfoResolver? TypeInfoResolver { get; set; }
    public IList<IJsonTypeInfoResolver> TypeInfoResolverChain { get; } =
        new List<IJsonTypeInfoResolver>();
    public bool AllowTrailingCommas { get; set; }
    public int DefaultBufferSize { get; set; }
    public JavaScriptEncoder? Encoder { get; set; }
    public JNaming? DictionaryKeyPolicy { get; set; }
    public bool IgnoreNullValues { get; set; }
    public JIgnore DefaultIgnoreCondition { get; set; }
    public JNumbers NumberHandling { get; set; }
    public JsonObjectCreationHandling PreferredObjectCreationHandling { get; set; }
    public bool IgnoreReadOnlyProperties { get; set; }
    public bool IgnoreReadOnlyFields { get; set; }
    public bool IncludeFields { get; set; }
    public int MaxDepth { get; set; }
    public JNaming? PropertyNamingPolicy { get; set; }
    public bool PropertyNameCaseInsensitive { get; set; }
    public JComments ReadCommentHandling { get; set; }
    public JUnknownTypes UnknownTypeHandling { get; set; }
    public JsonUnmappedMemberHandling UnmappedMemberHandling { get; set; }
    public bool WriteIndented { get; set; }
    public ReferenceHandler? ReferenceHandler { get; set; }
    public bool IsReadOnly { get; }

    public static implicit operator Jso(JsonSerializerOptionsBuilder builder) => builder.Build();

    public Jso Build()
    {
        var options = new Jso();
        foreach (var converter in Converters)
        {
            options.Converters.Add(converter);
        }

        return options;
    }

    public IList<JConverter> Converters { get; set; } = new List<JConverter>();
}

public partial interface IJsonSerializerOptions
{
    IList<JConverter> Converters { get; }
    IJsonTypeInfoResolver? TypeInfoResolver { get; set; }
    IList<IJsonTypeInfoResolver> TypeInfoResolverChain { get; }
    bool AllowTrailingCommas { get; set; }
    int DefaultBufferSize { get; set; }
    JavaScriptEncoder? Encoder { get; set; }
    JNaming? DictionaryKeyPolicy { get; set; }
    bool IgnoreNullValues { get; set; }
    JIgnore DefaultIgnoreCondition { get; set; }
    JNumbers NumberHandling { get; set; }
    JsonObjectCreationHandling PreferredObjectCreationHandling { get; set; }
    bool IgnoreReadOnlyProperties { get; set; }
    bool IgnoreReadOnlyFields { get; set; }
    bool IncludeFields { get; set; }
    int MaxDepth { get; set; }
    JNaming? PropertyNamingPolicy { get; set; }
    bool PropertyNameCaseInsensitive { get; set; }
    JComments ReadCommentHandling { get; set; }
    JUnknownTypes UnknownTypeHandling { get; set; }
    JsonUnmappedMemberHandling UnmappedMemberHandling { get; set; }
    bool WriteIndented { get; set; }
    ReferenceHandler? ReferenceHandler { get; set; }
    bool IsReadOnly { get; }
}
