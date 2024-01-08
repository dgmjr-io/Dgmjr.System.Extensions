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

public static class HostApplicationBuilderIdentityExtensions
{
    public static IHostApplicationBuilder AddAzureAdB2C(this IHostApplicationBuilder builder)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        IdentityModelEventSource.ShowPII = true;
        IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
        IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        var initialScopes = builder.Configuration.GetValue<string[]>(
            DownstreamApis_MsGraph_ScopesConfigurationKey
        );

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
            .AddDistributedTokenCaches()
            // .Services.AddScoped<IVerifiableCredentialsService, VerifiableCredentialsService>()
            // .ConfigureAll(
            //     delegate(DownstreamApiOptions downstreamApiOptions)
            //     {
            //         downstreamApiOptions.Serializer = (object? requestObject) =>
            //             new StringContent(
            //                 JsonSerializer.Serialize(
            //                     requestObject,
            //                     builder.Services
            //                         .BuildServiceProvider()
            //                         .CreateScope()
            //                         .ServiceProvider.GetRequiredService<
            //                             IOptionsMonitor<JsonOptions>
            //                         >()
            //                         .CurrentValue.JsonSerializerOptions
            //                 )
            //             );
            //     }
            // )
            .Services.Configure<MicrosoftIdentityApplicationOptions>(
                builder.Configuration.GetSection(AzureAdB2C)
            )
            .AddAuthorization()
            .AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        return builder;
    }
}
