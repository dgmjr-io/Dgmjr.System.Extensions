namespace System.Text.Json;
using System.Text;

/// <summary>
/// Utilities for parsing JSON.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// Parses binary data as JSON.
    /// </summary>
    public static JsonElement Parse(byte[] json, JsonDocumentOptions options = default)
    {
        using var document = JsonDocument.Parse(json, options);
        return document.RootElement.Clone();
    }

    /// <summary>
    /// Parses string as JSON.
    /// </summary>
    public static JsonElement Parse(string json, JsonDocumentOptions options = default) =>
        Parse(UTF8.GetBytes(json), options);

    /// <summary>
    /// Tries to parse binary data as JSON or returns null if the input is malformed.
    /// </summary>
    public static JsonElement? TryParse(byte[] json, JsonDocumentOptions options = default)
    {
        try
        {
            return Parse(json, options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to parse string as JSON or returns null if the input is malformed.
    /// </summary>
    public static JsonElement? TryParse(string json, JsonDocumentOptions options = default) =>
        TryParse(UTF8.GetBytes(json), options);
}
