namespace Dgmjr.Redis.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Net;
using MSRedisCacheOptions = Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions;

public class RedisEndpointCollectionProvider(IConfiguration configuration) : IConfigureOptions<RedisCacheOptions>, IConfigureOptions<MSRedisCacheOptions>
{
    private const int DefaultPort = 6379;
private readonly IConfiguration _configuration = configuration;
private const string EndPointsSectionName = $"{nameof(RedisCacheOptions.ConfigurationOptions)}:{nameof(ConfigurationOptions.EndPoints)}";

public void Configure(RedisCacheOptions options)
{
    Configure(options as MSRedisCacheOptions);
}

public void Configure(MSRedisCacheOptions options)
{
    if (options is null)
    {
        throw new ArgumentNullException(nameof(options));
    }

    options.ConfigurationOptions ??= new ConfigurationOptions();

    // Retrieve the configuration values from appsettings.json or any other configuration source
    var endpointConfig = _configuration.GetSection(EndPointsSectionName);

    foreach (var endpointSection in endpointConfig.GetChildren())
    {
        var host = endpointSection.GetValue<string?>(nameof(DnsEndPoint.Host));
        var port = endpointSection.GetValue<int?>(nameof(DnsEndPoint.Port));
        var ipAddress = endpointSection.GetValue<string?>(nameof(IPEndPoint.Address));
        if (!IsNullOrEmpty(ipAddress))
        {
            options.ConfigurationOptions?.EndPoints?.Add(new IPEndPoint(IPAddress.Parse(ipAddress), port ?? DefaultPort));
        }
        else if (!IsNullOrEmpty(host))
        {
            options.ConfigurationOptions?.EndPoints?.Add(new DnsEndPoint(host, port ?? DefaultPort));
        }
        else
        {
            throw new InvalidOperationException($"Invalid endpoint configuration: {endpointSection.Key}");
        }
    }
}
}
