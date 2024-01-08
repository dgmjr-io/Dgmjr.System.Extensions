using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Abstractions;

namespace Dgmjr.AzureAd;

public static class Constants
{
    public const string DownstreamApis = nameof(DownstreamApis);
    public const string MicrosoftGraphOptions = nameof(MicrosoftGraphOptions);
    public const string MicrosoftGraph = nameof(MicrosoftGraph);
    public const string Scopes = nameof(Scopes);
    public const string AzureAdB2C = Microsoft.Identity.Web.Constants.AzureAdB2C;
    public const string AzureAd = Microsoft.Identity.Web.Constants.AzureAd;
    public const string OpenIdConnect = OpenIdConnectDefaults.AuthenticationScheme;

    public const string DownstreamApis_MsGraph_ScopesConfigurationKey = DownstreamApis + ":" + MicrosoftGraph + ":" + Scopes;
    public const string DownstreamApis_MsGraphConfigurationKey = DownstreamApis + ":" + MicrosoftGraph;
}
