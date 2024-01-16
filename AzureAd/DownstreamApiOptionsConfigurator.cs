namespace Dgmjr.AzureAd;
using System.Net.Http;

public class DownstreamApiOptionsConfigurator(IOptionsMonitor<JsonOptions> jsonOptions) : IConfigureOptions<DownstreamApiOptions>
{
    private readonly JsonOptions _jsonOptions = jsonOptions.CurrentValue;

public void Configure(DownstreamApiOptions options)
{
    options.AcquireTokenOptions ??= new();
    options.Serializer = requestObject =>
        new StringContent(
            Serialize(
                requestObject,
                _jsonOptions.JsonSerializerOptions
            ),
            UTF8,
            ApplicationMediaTypeNames.Json
        );
    options.CustomizeHttpRequestMessage += (message) =>
    {
        message.Headers.Add(HReqH.Accept.DisplayName, Application.Json.DisplayName);
        message.Headers.Add(HReqH.ContentType.DisplayName, Application.Json.DisplayName);
        message.Headers.Add(HReqH.AcceptCharset.DisplayName, UTF8.HeaderName);
    };
}
}
