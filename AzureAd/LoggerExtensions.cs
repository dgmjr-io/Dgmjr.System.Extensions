using Dgmjr.Identity.Web;

using Microsoft.Extensions.Logging;

namespace Dgmjr.AzureAd;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Trace,
        Message = "Registering Microsoft Identity Web UI.",
        EventName = nameof(RegisteringIdentityWebUi)
    )]

    public static partial void RegisteringIdentityWebUi(
        this ILogger logger
    );
    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Trace,
        Message = "Registering app of type {AppType}",
        EventName = nameof(RegisteringAppWithType)
    )]
    public static partial void RegisteringAppWithType(
        this ILogger logger,
        AppType appType
    );
}
