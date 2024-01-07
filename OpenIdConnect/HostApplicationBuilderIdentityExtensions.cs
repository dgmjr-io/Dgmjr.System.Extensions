namespace Microsoft.Extensions.DependencyInjection;

using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;

public static class HostApplicationBuilderIdentityExtensions
{
    public static IHostApplicationBuilder AddAzureAdB2C(
        this IHostApplicationBuilder builder
    )
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        IdentityModelEventSource.ShowPII = true;
        IdentityModelEventSource.Logger.LogLevel = EventLevel.Verbose;
        IdentityModelEventSource.LogCompleteSecurityArtifact = true;
        var initialScopes = builder.Configuration.GetValue<string[]>(DownstreamApisMsGraphScopesConfigurationKey);
        builder.Services
            .AddAuthentication(OpenIdConnect)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(
                builder.Configuration.GetSection("DownstreamApis:MicrosoftGraphOptions")
            )
            .AddDownstreamApi(
                "MicrosoftGraphOptions",
                builder.Configuration.GetSection("DownstreamApis:MicrosoftGraphOptions")
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
            .Configure<MicrosoftIdentityApplicationOptions>(
                builder.Configuration.GetSection("AzureAdB2C")
            )
            .AddAuthorization()
            .AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        return builder;
    }
}
