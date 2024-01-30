namespace Microsoft.Extensions.DependencyInjection;

using Dgmjr.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using Serilog;

public class AutomaticLoggingConfigurator
    : IConfigureIHostApplicationBuilder,
        IConfigureIApplicationBuilder
{
    public ConfigurationOrder Order => ConfigurationOrder.VeryEarly;

    public void Configure(WebApplicationBuilder builder)
    {
        builder.AddLoggingAndApplicationInsightsTelemetry();
        // builder.Services.Add
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpLogging();
        app.UseSerilogRequestLogging();
    }
}
