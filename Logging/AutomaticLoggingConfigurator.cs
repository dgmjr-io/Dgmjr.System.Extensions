#if NET6_0_OR_GREATER
namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

public class AutomaticLoggingConfigurator
    : Microsoft.AspNetCore.Builder.ConfiguratorBase<AutomaticLoggingConfigurator>
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        services.AddSingleton<
            Serilog.IDiagnosticContext,
            Serilog.Extensions.Hosting.DiagnosticContext
        >();
        services.AddLoggingAndApplicationInsightsTelemetry(Configuration);
        services.AddSingleton<IStartupFilter>(this);
    }

    protected override void Configure(IApplicationBuilder app)
    {
        app.UseHttpLogging();
        app.UseSerilogRequestLogging();
    }

    // public void Configure(IWebHostBuilder builder)
    // {
    //     builder.ConfigureLogging(
    //         (context, logging) =>
    //         {
    //             Log.Logger = new LoggerConfiguration().ReadFrom
    //                 .Configuration(context.Configuration)
    //                 .CreateBootstrapLogger();
    //         }
    //     );
    //     builder.ConfigureServices(
    //         (context, services) =>
    //         {
    //             services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
    //             services.AddSingleton<Serilog.Extensions.Hosting.DiagnosticContext>();
    //             services.AddLoggingAndApplicationInsightsTelemetry(context.Configuration);
    //             services.AddSingleton<IStartupFilter>(this);
    //         }
    //     );
    // }

    // public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    // {
    //     return app =>
    //     {
    //         app.UseHttpLogging();
    //         app.UseSerilogRequestLogging();
    //         next(app);
    //     };
    // }
}
#endif
