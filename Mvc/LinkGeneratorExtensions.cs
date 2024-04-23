namespace Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public enum HttpProtocol
{
    Http,
    Https
}

public static class LinkGeneratorExtensions
{
    public static string? GetActionUri(
        this IUrlHelper linkGenerator,
        string actionName,
        string controllerName,
        object? routeValues = default,
        HttpProtocol scheme = HttpProtocol.Https,
        HostString? host = default,
        // PathString pathBase = default,
        FragmentString? fragment = default //,
    // LinkOptions? options = null
    ) =>
        linkGenerator.Action(
            new Routing.UrlActionContext
            {
                Action = actionName,
                Controller = controllerName?.Replace(nameof(Controller), ""),
                Values = routeValues,
                Host =
                    host?.Value ?? linkGenerator.ActionContext.HttpContext.Request.Host.ToString(),
                Protocol = scheme.ToString().ToLower(),
                Fragment = fragment.ToString()
            }
        );

    public static string? GetActionUri<TController>(
        this IUrlHelper linkGenerator,
        string actionName,
        object? routeValues = default,
        HttpProtocol scheme = HttpProtocol.Https,
        HostString? host = default,
        FragmentString? fragment = default
    )
        where TController : ControllerBase =>
        linkGenerator.GetActionUri(
            actionName,
            typeof(TController).Name.Replace(nameof(Controller), ""),
            routeValues,
            scheme,
            host ?? linkGenerator.ActionContext.HttpContext.Request.Host,
            fragment
        );

    // public static string? GetActionUri(this LinkGenerator linkGenerator, Expression<Action> action,
    //     string? scheme,
    //     HostString host,
    //     PathString pathBase = default,
    //     FragmentString fragment = default,
    //     LinkOptions? options = null)
    // => linkGenerator.GetActionUri(action.AsMethod().Name, action.AsMethod().DeclaringType.Name, action.Parameters.ToDictionary(p => p.Name, p => p.))
}
