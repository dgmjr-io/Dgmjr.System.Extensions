namespace Microsoft.Extensions.DependencyInjection;

using Dgmjr.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

public class AutomaticLoggingConfigurator
    : IConfigureIHostApplicationBuilder,
        IConfigureIApplicationBuilder
{
    public ConfigurationOrder Order => ConfigurationOrder.VeryEarly;

    public void Configure(IHostApplicationBuilder builder)
    {
        builder.AddLoggingAndApplicationInsightsTelemetry();
    }

    public void Configure(IApplicationBuilder app) { }
}
