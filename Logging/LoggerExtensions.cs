namespace Microsoft.Extensions.Logging;

using System.Net.Http;

using Serilog;

using static System.Net.Http.HttpMethod;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{Method} {Path}",
        EventName = nameof(PageVisited)
    )]
    public static partial void PageVisited(this ILogger logger, HttpMethod method, string path);

    public static void PageVisited(this ILogger logger, string method, string path) =>
        logger.PageVisited(new HttpMethod(method), path);

    public static void Get(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Get, path);

    public static void Delete(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Delete, path);

    public static void Post(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Post, path);

    public static void Patch(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Patch)), path);

    public static void Put(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Put, path);

    public static void Options(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Options, path);

    public static void Head(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Head, path);

    public static void Trace(this ILogger logger, string path) =>
        logger.PageVisited(HttpMethod.Trace, path);

    public static void Connect(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Connect).ToUpper()), path);

    public static void Move(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Move).ToUpper()), path);

    public static void Copy(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Copy).ToUpper()), path);

    public static void Link(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Link).ToUpper()), path);

    public static void Unlink(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Unlink).ToUpper()), path);

    public static void Lock(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Lock).ToUpper()), path);

    public static void Unlock(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Unlock).ToUpper()), path);

    public static void Propfind(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Propfind).ToUpper()), path);

    public static void View(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(View).ToUpper()), path);

    public static void Search(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Search).ToUpper()), path);

    public static void Report(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Report).ToUpper()), path);

    public static void Checkout(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Checkout).ToUpper()), path);

    public static void Merge(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Merge).ToUpper()), path);

    public static void Label(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Label).ToUpper()), path);

    public static void Update(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Update).ToUpper()), path);

    public static void Mkactivity(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Mkactivity).ToUpper()), path);

    public static void Mkcol(this ILogger logger, string path) =>
        logger.PageVisited(new HttpMethod(nameof(Mkcol).ToUpper()), path);
}
