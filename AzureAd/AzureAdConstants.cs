namespace Dgmjr.AzureAd;

public static class Constants
{
    /// <value>DownstreamApis</value>
    public const string DownstreamApis = nameof(DownstreamApis);
    /// <value>MicrosoftGraphOptions</value>
    public const string MicrosoftGraphOptions = nameof(MicrosoftGraphOptions);
    /// <value>MicrosoftGraph</value>
    public const string MicrosoftGraph = nameof(MicrosoftGraph);
    /// <value>Scopes</value>
    public const string Scopes = nameof(Scopes);
    /// <value>AzureAdB2C</value>
    public const string AzureAdB2C = Microsoft.Identity.Web.Constants.AzureAdB2C;
    /// <value>AzureAd</value>
    public const string AzureAd = Microsoft.Identity.Web.Constants.AzureAd;
    /// <value>OpenIdConnect</value>
    public const string OpenIdConnect = OpenIdConnectDefaults.AuthenticationScheme;

    /// <value><inheritdoc cref="DownstreamApis" path="/value" />:<inheritdoc cref="MicrosoftGraph" path="/value" />:<inheritdoc cref="Scopes" path="/value" /></value>
    public const string DownstreamApis_MsGraph_ScopesConfigurationKey = DownstreamApis + ":" + MicrosoftGraph + ":" + Scopes;
    /// <value><inheritdoc cref="DownstreamApis" path="/value" />:<inheritdoc cref="MicrosoftGraph" path="/value" /></value>
    public const string DownstreamApis_MsGraphConfigurationKey = DownstreamApis + ":" + MicrosoftGraph;
}
