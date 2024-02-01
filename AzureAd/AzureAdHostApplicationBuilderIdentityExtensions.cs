namespace Microsoft.Extensions.DependencyInjection;

using System.Net.Http;

using Dgmjr.AzureAd.Web;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Web.UI;
using MicrosoftIdentityOptions = Dgmjr.AzureAd.Web.MicrosoftIdentityOptions;
using MsidCallsWebApiAuthBuilder = MicrosoftIdentityAppCallsWebApiAuthenticationBuilder;

public static class AzureAdHostApplicationBuilderIdentityExtensions
{
    public static WebApplicationBuilder AddAzureAdB2CIdentity(this WebApplicationBuilder builder)
    {
        if (
            builder.Services.Any(
                sd => sd.ServiceType == typeof(MicrosoftIdentityIssuerValidatorFactory)
            )
        )
        {
            return builder; // we've already added the services
        }

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

        MsidCallsWebApiAuthBuilder callsWebApiAuthenticationBuilder;
        if (AppType.WebUiBased.HasFlag(options.AppType))
        {
            Console.WriteLine("Registering Microsoft Identity Web UI.");
            callsWebApiAuthenticationBuilder = authenticationBuilder
                .AddMicrosoftIdentityWebApp(configurationSection)
                .EnableTokenAcquisitionToCallDownstreamApi(opts => configurationSection.Bind(opts), options.Scope);
            builder.Services.AddMvc().AddMicrosoftIdentityUI();
        }
        else if (AppType.ApiBased.HasFlag(options.AppType))
        {
            Console.WriteLine("Registering app with type {0}", options.AppType);
            callsWebApiAuthenticationBuilder = authenticationBuilder
                .AddMicrosoftIdentityWebApi(configurationSection)
                .EnableTokenAcquisitionToCallDownstreamApi(opts => configurationSection.Bind(opts));
        }
        else
        {
            throw new InvalidOperationException(
                $"Invalid AppType value \"{options.AppType}\" for AzureAdB2C configuration."
            );
        }

        var msGraphOptionsConfigSection = builder.Configuration.GetSection(DownstreamApis_MsGraphConfigurationKey);
        var msGraphOptions = msGraphOptionsConfigSection.Get<AzureAdB2CGraphOptions>();

        authenticationBuilder.AddJwtBearer(
            JwtBearerSchemeName,
            JwtBearerSchemeDisplayName,
            options => configurationSection.Bind(options)
        );

        if(msGraphOptions.AppOnly)
        {
            callsWebApiAuthenticationBuilder
                .AddMicrosoftGraphAppOnly(authProvider => new GraphServiceClient(authProvider))
                .AddDistributedTokenCaches();
        }
        else
        {
            callsWebApiAuthenticationBuilder
                .AddMicrosoftGraph(msGraphOptionsConfigSection)
                .AddDistributedTokenCaches();
        }

        // callsWebApiAuthenticationBuilder.AddSessionTokenCaches();

        foreach (
            var downstreamApiConfig in builder.Configuration
                .GetSection(Dgmjr.AzureAd.Constants.DownstreamApis)
                .GetChildren()
        )
        {
            callsWebApiAuthenticationBuilder.AddDownstreamApi(
                downstreamApiConfig.Key,
                options =>
                {
                    downstreamApiConfig.Bind(options);
                    options.Serializer = requestObject =>
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
                        );
                }
            );
        }

        builder.Services
            .AddMicrosoftIdentityConsentHandler()
            .AddTransient<Microsoft.Identity.Web.UI.Areas.MicrosoftIdentity.Controllers.AccountController>();
        builder.Services
            // .ConfigureAll<DownstreamApiOptions>(
            //     downstreamApiOptions =>
            //         downstreamApiOptions.Serializer = requestObject =>
            //             new StringContent(
            //                 Serialize(
            //                     requestObject,
            //                     builder.Services
            //                         .BuildServiceProvider()
            //                         .CreateScope()
            //                         .ServiceProvider.GetRequiredService<
            //                             IOptionsMonitor<JsonOptions>
            //                         >()
            //                         .CurrentValue.JsonSerializerOptions
            //                 )
            //             )
            // )
            .Configure<MicrosoftIdentityApplicationOptions>(
                builder.Configuration.GetSection(AzureAdB2C)
            )
            .Configure<Dgmjr.AzureAd.Web.MicrosoftIdentityOptions>(
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
