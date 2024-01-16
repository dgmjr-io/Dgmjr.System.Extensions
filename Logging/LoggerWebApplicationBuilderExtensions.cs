namespace Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Configuration;

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using global::Azure.Monitor.OpenTelemetry.AspNetCore;
using global::Azure.Identity;

public static partial class LoggingWebApplicationBuilderExtensions
{
    private const string Logging = nameof(Logging);
    private const string OpenTelemetryResourceAttributes = nameof(OpenTelemetryResourceAttributes);
    private const string ApplicationInsights = nameof(ApplicationInsights);
    private const string HttpLogging = nameof(HttpLogging);
    private const string ConnectionString = nameof(ConnectionString);
    private const string ApplicationInsightsConnectionString = $"{ApplicationInsights}:{ConnectionString}";

#if NET5_0_OR_GREATER
    public static IHostApplicationBuilder AddLoggingAndApplicationInsightsTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Services.AddLoggingAndApplicationInsightsTelemetry(builder.Configuration);
        builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: config =>
                config.ConnectionString = builder.Configuration.GetConnectionString(
                    ApplicationInsights
                ),
            configureApplicationInsightsLoggerOptions: _ => {  }
        );

        builder.Logging
#if DEBUG
                    .AddConsole()
                    .AddDebug()
#endif
                    .AddConfiguration(builder.Configuration.GetSection(Logging));

        return builder;
    }
#endif
    public static IServiceCollection AddLoggingAndApplicationInsightsTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        TelemetryConfiguration.Active.ConnectionString = configuration
            .GetValue<string>(ApplicationInsightsConnectionString);
        services
            .AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.Credential = new DefaultAzureCredential();
                options.ConnectionString = configuration.GetConnectionString(
                    ApplicationInsights
                );
            });

#if NET5_0_OR_GREATER
        services.AddHttpLogging(
            options =>
                configuration.GetRequiredSection(HttpLogging).Bind(options)
        );
#endif
        services.AddApplicationInsightsTelemetry(
            configuration.GetSection(ApplicationInsights)
        );

        services.ConfigureOpenTelemetryTracerProvider(
            (sp, builder2) =>
                builder2.ConfigureResource(
                    resourceBuilder =>
                        resourceBuilder.AddAttributes(
                            configuration
                                .GetSection(OpenTelemetryResourceAttributes)
                                .Get<Dictionary<string, object>>()!
                        )
                )
        );
        return services;
    }
}
