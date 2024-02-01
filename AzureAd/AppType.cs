namespace Dgmjr.AzureAd.Web;

[Flags]
public enum AppType
{
    Web = 1,

    // Api = 2,
    WebApi = 4,
    RazorPages = 8,
    Mvc = 16,
    Console = 32,
    Worker = 64,
    Service = 128,
    Function = 256,
    AzureFunction = 512,
    AzureWebJob = 1024,

    ApiBased = WebApi | AzureFunction,

    WebUiBased = Web | RazorPages | Mvc,

    WebBased = ApiBased | WebUiBased,

    All =
        Web
        | RazorPages
        | Mvc
        | WebApi
        | AzureFunction
        | AzureWebJob
        | Console
        | Worker
        | Service
        | Function,

    DesktopBased = Console | Worker | Service
}
