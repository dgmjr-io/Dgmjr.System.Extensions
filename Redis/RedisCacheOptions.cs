using StackExchange.Redis;

namespace Dgmjr.Redis.Extensions;

public class RedisCacheOptions : Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions
{
    public const string Redis = nameof(Redis);

    public bool UseRedis { get; set; } = false;

    /// <summary>Gets or sets a connection string for the redis cache</summary>
    public string? ConnectionString
    {
        get => IsNullOrWhiteSpace(Configuration) ? ConfigurationOptions?.ToString() : Configuration;
        set => ConfigurationOptions = ConfigurationOptions.Parse(value);
    }

    /// <summary>Gets whether a connection string was specified.</summary>
    /// <returns><see langword="true" /> if a connection string was specified, <see langword="false" /> otherwise.<returns>
    public bool IsUsingConnectionString => !IsNullOrWhiteSpace(Configuration);
}
