namespace Microsoft.Extensions.DependencyInjection;

public class AutomaticAzureAdConfigurator : IConfigureIHostApplicationBuilder, IConfigureIApplicationBuilder
{
    public ConfigurationOrder Order => ConfigurationOrder.AnyTime;

    public void Configure(IHostApplicationBuilder builder)
    {
        builder.AddAzureAdB2CIdentity();
    }

    public void Configure(IApplicationBuilder options)
    {
        options.UseAzureAdB2CIdentity();
    }
}
