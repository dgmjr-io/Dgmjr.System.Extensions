namespace Dgmjr.Identity.Web;

public class MicrosoftIdentityOptions : Microsoft.Identity.Web.MicrosoftIdentityOptions
{
    public AppType AppType { get; set; } = AppType.WebApi;
}
