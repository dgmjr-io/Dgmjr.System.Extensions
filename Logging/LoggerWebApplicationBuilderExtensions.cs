#if NET6_0_OR_GREATER
namespace Microsoft.Extensions.DependencyInjection;

using global::Azure.Identity;
using global::Azure.Monitor.OpenTelemetry.AspNetCore;
using global::Azure.Monitor.OpenTelemetry.Exporter;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Configuration;

using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Serilog;

using ILogger = Microsoft.Extensions.Logging.ILogger;

public static partial class LoggingWebApplicationBuilderExtensions
{
    private const string Logging = nameof(Logging);
    private const string OpenTelemetryResourceAttributes = nameof(OpenTelemetryResourceAttributes);
    private const string ApplicationInsights = nameof(ApplicationInsights);
    private const string HttpLogging = nameof(HttpLogging);
    private const string ConnectionString = nameof(ConnectionString);
    private const string ApplicationInsightsConnectionString =
        $"{ApplicationInsights}:{ConnectionString}";

    public static WebApplicationBuilder AddLoggingAndApplicationInsightsTelemetry(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.AddLoggingAndApplicationInsightsTelemetry(
            builder.Host,
            builder.Logging,
            builder.Configuration
        );
        return builder;
    }

    public static IServiceCollection AddLoggingAndApplicationInsightsTelemetry(
        this IServiceCollection services,
        IHostBuilder host,
        ILoggingBuilder logging,
        IConfiguration configuration
    )
    {
        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(configuration)
            .CreateBootstrapLogger();

        host.UseSerilog(Log.Logger, true);

        services.AddLoggingAndApplicationInsightsTelemetry(configuration);
        logging
            .AddApplicationInsights(
                configureTelemetryConfiguration: config =>
                    config.ConnectionString = configuration.GetConnectionString(
                        ApplicationInsights
                    ),
                configureApplicationInsightsLoggerOptions: _ => { }
            )
#if DEBUG
            .AddConsole()
            .AddDebug()
#endif
            .AddOpenTelemetry()
            .AddSerilog()
            .AddAzureWebAppDiagnostics()
            // .AddEventLog()
            .AddEventSourceLogger();

        services.AddTransient<ILogger>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<object>>();
            return logger;
        });

        return services;
    }

    public static IServiceCollection AddLoggingAndApplicationInsightsTelemetry(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var appInsightsConnectionString = configuration.GetConnectionString(ApplicationInsights);
        // TelemetryConfiguration.Active.ConnectionString = appInsightsConnectionString;
        // services
        //     .Configure<TelemetryConfiguration>(
        //         options => options.ConnectionString = appInsightsConnectionString
        //     )
        //     // .Configure<AzureMonitorExporterOptions>(
        //     //     options => options.ConnectionString = appInsightsConnectionString
        //     // )
        //     .AddOpenTelemetry()
        //     // .UseAzureMonitor(options =>
        //     // {
        //     //     options.Credential = new DefaultAzureCredential();
        //     //     options.ConnectionString = appInsightsConnectionString;
        //     // })
        //     .ConfigureResource(resourceBuilder =>
        //     {
        //         resourceBuilder.AddAttributes(
        //             configuration
        //                 .GetSection(OpenTelemetryResourceAttributes)
        //                 .Get<Dictionary<string, object>>()!
        //         );
        //     })
        //     .WithMetrics(
        //         builder =>
        //             builder
        //                 .AddHttpClientInstrumentation()
        //                 .AddAspNetCoreInstrumentation()
        //                 .AddOtlpExporter()
        //     )
        //     .WithTracing(
        //         builder =>
        //             builder
        //                 .AddAspNetCoreInstrumentation(o =>
        //                 {
        //                     o.EnrichWithHttpRequest = (activity, httpRequest) =>
        //                         activity.SetTag("requestProtocol", httpRequest.Protocol);
        //                     o.EnrichWithHttpResponse = (activity, httpResponse) =>
        //                         activity.SetTag("responseLength", httpResponse.ContentLength);
        //                     o.EnrichWithException = (activity, exception) =>
        //                         activity.SetTag("exceptionType", exception.GetType().ToString());
        //                 })
        //                 .AddConsoleExporter()
        //     );

        services.AddHttpLogging(
            options => configuration.GetRequiredSection(HttpLogging).Bind(options)
        );
        // services.AddApplicationInsightsTelemetry(configuration.GetSection(ApplicationInsights));

        services.ConfigureOpenTelemetryTracerProvider(
            (_, traceBuilder) =>
            {
                traceBuilder.ConfigureResource(
                    resourceBuilder =>
                        resourceBuilder.AddAttributes(
                            configuration
                                .GetSection(OpenTelemetryResourceAttributes)
                                .Get<Dictionary<string, object>>()!
                        )
                );
            }
        );
        services.ConfigureOpenTelemetryMeterProvider(
            (_, meterBuilder) =>
            {
                meterBuilder.ConfigureResource(
                    resourceBuilder =>
                        resourceBuilder.AddAttributes(
                            configuration
                                .GetSection(OpenTelemetryResourceAttributes)
                                .Get<Dictionary<string, object>>()!
                        )
                );
            }
        );
        return services;
    }

    public static WebApplicationBuilder UseLogging(
        this WebApplicationBuilder builder,
        Serilog.ILogger? logger = null,
        bool dispose = false
    )
    {
        Log.Logger ??= logger ?? StaticLogger.NewSerilogger();
        builder.Host.UseSerilog(logger, dispose);
        return builder;
    }
}
#endif
