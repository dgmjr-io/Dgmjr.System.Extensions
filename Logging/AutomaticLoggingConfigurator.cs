namespace Microsoft.Extensions.DependencyInjection;
using Dgmjr.Configuration.Extensions;

using Microsoft.Extensions.Hosting;

public class AutomaticLoggingConfigurator : IConfigureIHostApplicationBuilder
{
    public ConfigurationOrder Order => ConfigurationOrder.VeryEarly;

    public void Configure(IHostApplicationBuilder builder)
    {
        builder.AddLoggingAndApplicationInsightsTelemetry();
    }
}
