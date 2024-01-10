namespace Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

public static class HostApplicationBuilderIdentityExtensions
{
    public static IHostApplicationBuilder AddAzureAdB2CIdentity(
        this IHostApplicationBuilder builder
    )
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        IdentityModelEventSource.ShowPII = true;
        IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
        IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        var initialScopes = builder.Configuration.GetValue<string[]>(
            DownstreamApis_MsGraph_ScopesConfigurationKey
        )!;
        builder.Services
            .AddAuthentication(OpenIdConnect)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection(AzureAdB2C))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(
                builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey)
            )
            .AddDownstreamApi(
                MicrosoftGraph,
                builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey)
            )
            .AddDistributedTokenCaches().Services
            // .Services.AddScoped<IVerifiableCredentialsService, VerifiableCredentialsService>()
            .ConfigureAll<DownstreamApiOptions>(
                downstreamApiOptions =>
                {
                    downstreamApiOptions.Serializer = requestObject =>
                        new StringContent(
                            System.Text.Json.JsonSerializer.Serialize(
                                requestObject,
                                builder.Services
                                    .BuildServiceProvider()
                                    .CreateScope()
                                    .ServiceProvider.GetRequiredService<IOptionsMonitor<JsonOptions>>()
                                    .CurrentValue.JsonSerializerOptions
                            )
                        );
                }
            )
            .Configure<MicrosoftIdentityApplicationOptions>(
                builder.Configuration.GetSection(AzureAdB2C)
            )
            .Configure<Microsoft.Identity.Web.MicrosoftGraphOptions>(
                builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey)
            )
            .AddAuthorization()
            .AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        return builder;
    }
}
