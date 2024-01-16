namespace Microsoft.Extensions.Logging;
using System.Net.Http;
using static System.Net.Http.HttpMethod;

public static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{Method} {Path}", EventName = nameof(PageVisited))]
    public static partial void PageVisited(this ILogger logger, HttpMethod method, string path);
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{Method} {Path}", EventName = nameof(PageVisited))]
    public static partial void PageVisited(this ILogger logger, string method, string path);

    public static void Get(this ILogger logger, string path)
        => logger.PageVisited(HttpMethod.Get, path);
    public static void Delete(this ILogger logger, string path)
        => logger.PageVisited(HttpMethod.Delete, path);
    public static void Post(this ILogger logger, string path)
        => logger.PageVisited(HttpMethod.Post, path);
    public static void Patch(this ILogger logger, string path)
        => logger.PageVisited(HttpMethod.Patch, path);
}
