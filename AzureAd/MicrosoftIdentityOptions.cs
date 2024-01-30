namespace Dgmjr.AzureAd.Web;

public class MicrosoftIdentityOptions : Microsoft.Identity.Web.MicrosoftIdentityOptions
{
    public AppType AppType { get; set; } = AppType.WebApi;
    public string DefaultFallbackRoute { get; set; } = "/index";
    public string[] InitialScopes { get; set; } = Empty<string>();
}
