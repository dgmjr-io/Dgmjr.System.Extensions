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
    public static JElem Parse(byte[] json, JDocOpts options = default)
    {
        using var document = JDoc.Parse(json, options);
        return document.RootElement.Clone();
    }

    /// <summary>
    /// Parses string as JSON.
    /// </summary>
    public static JElem Parse(string json, JDocOpts options = default) =>
        Parse(UTF8.GetBytes(json), options);

    /// <summary>
    /// Tries to parse binary data as JSON or returns null if the input is malformed.
    /// </summary>
    public static JElem? TryParse(byte[] json, JDocOpts options = default)
    {
        try
        {
            return Parse(json, options);
        }
        catch (JException)
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to parse string as JSON or returns null if the input is malformed.
    /// </summary>
    public static JElem? TryParse(string json, JDocOpts options = default) =>
        TryParse(UTF8.GetBytes(json), options);
}
