namespace Dgmjr.Caching.Extensions;

using static Microsoft.Extensions.Logging.LogLevel;

public static partial class LoggerExtensions
{
    [LoggerMessage(Information, "Cache miss for key {key}", EventName = "CacheMiss")]
    public static partial void CacheMiss(this ILogger logger, string key);

    [LoggerMessage(Information, "Cache hit for key {key}", EventName = "CacheHit")]
    public static partial void CacheHit(this ILogger logger, string key);
}
