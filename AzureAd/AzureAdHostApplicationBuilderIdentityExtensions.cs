namespace Microsoft.Extensions.DependencyInjection;

using System.Net.Http;

using Dgmjr.Identity.Web;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web.UI;
using MicrosoftIdentityOptions = Dgmjr.Identity.Web.MicrosoftIdentityOptions;

public static class AzureAdHostApplicationBuilderIdentityExtensions
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
        var configurationSection = builder.Configuration.GetSection(AzureAdB2C);
        var options = configurationSection.Get<MicrosoftIdentityOptions>();

        var authenticationBuilder = builder.Services.AddAuthentication(OpenIdConnect);

        MicrosoftIdentityAppCallsWebApiAuthenticationBuilder callsWebApiAuthenticationBuilder;
        if ((options.AppType & AppType.WebBased) == options.AppType)
        {
            Console.WriteLine("Registering Microsoft Identity Web UI.");
            callsWebApiAuthenticationBuilder = authenticationBuilder
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection(AzureAdB2C))
                .EnableTokenAcquisitionToCallDownstreamApi(options.Scope);
            builder.Services.AddMvc().AddMicrosoftIdentityUI();
        }
        else if ((options.AppType & AppType.ApiBased) == options.AppType)
        {
            Console.WriteLine("Registering app with type {0}", options.AppType);
            callsWebApiAuthenticationBuilder = authenticationBuilder
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(AzureAdB2C))
                .EnableTokenAcquisitionToCallDownstreamApi();
        }
        else
        {
            throw new InvalidOperationException(
                $"Invalid AppType value \"{options.AppType}\" for AzureAdB2C configuration."
            );
        }

        authenticationBuilder.AddJwtBearer(
            JwtBearerSchemeName,
            JwtBearerSchemeDisplayName,
            options => configurationSection.Bind(options)
        );

        callsWebApiAuthenticationBuilder
            .AddMicrosoftGraph(
                builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey)
            )
            .AddDistributedTokenCaches();

        foreach (
            var downstreamApiConfig in builder.Configuration
                .GetSection(DownstreamApis)
                .GetChildren()
        )
        {
            callsWebApiAuthenticationBuilder.AddDownstreamApi(
                downstreamApiConfig.Key,
                downstreamApiConfig
            );
        }

        builder.Services
            .AddMicrosoftIdentityConsentHandler()
            .AddTransient<Microsoft.Identity.Web.UI.Areas.MicrosoftIdentity.Controllers.AccountController>();
        builder.Services
            .ConfigureAll<DownstreamApiOptions>(
                downstreamApiOptions =>
                    downstreamApiOptions.Serializer = requestObject =>
                        new StringContent(
                            Serialize(
                                requestObject,
                                builder.Services
                                    .BuildServiceProvider()
                                    .CreateScope()
                                    .ServiceProvider.GetRequiredService<
                                        IOptionsMonitor<JsonOptions>
                                    >()
                                    .CurrentValue.JsonSerializerOptions
                            )
                        )
            )
            .Configure<MicrosoftIdentityApplicationOptions>(
                builder.Configuration.GetSection(AzureAdB2C)
            )
            .Configure<MicrosoftGraphOptions>(
                builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey)
            )
            .AddAuthorization()
            .AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>()
            .AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
        return builder;
    }
}