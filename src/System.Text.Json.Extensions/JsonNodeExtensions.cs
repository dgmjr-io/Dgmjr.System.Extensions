namespace System.Text.Json;

using System.Net.NetworkInformation;
using System.Text.Json.Nodes;

public static class JsonNodeExtensions
{
    public static T? As<T>(this JsonNode node, Jso jso)
        => node.Deserialize<T>(jso);
}
