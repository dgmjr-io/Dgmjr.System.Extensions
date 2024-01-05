namespace Dgmjr.Extensions.Caching;
using static Microsoft.Extensions.Logging.LogLevel;

public static partial class LoggerExtensions
{
    [LoggerMessage(Information, "Cache miss for key {key}", EventName = "CacheMiss")]
    public static partial void LogCacheMiss(this ILogger logger, string key);

    [LoggerMessage(Information, "Cache hit for key {key}", EventName = "CacheHit")]
    public static partial void LogCacheHit(this ILogger logger, string key);
}
