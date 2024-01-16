namespace Microsoft.Extensions.DependencyInjection;
using Dgmjr.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

public class RedisAutoConfigurator : IConfigureIHostApplicationBuilder, IConfigureIApplicationBuilder
{
    private const string Redis = nameof(Redis);
    private const string ResponseCaching = nameof(ResponseCaching);
    public ConfigurationOrder Order => ConfigurationOrder.AnyTime;

    public void Configure(IHostApplicationBuilder builder)
    {
        var redisOptions = builder.Configuration.GetSection(Redis).Get<RedisCacheOptions>();
        if (redisOptions?.UseRedis == true)
        {
            builder.AddRedisCaching();
        }
    }

    public void Configure(IApplicationBuilder builder)
    {
        var redisOptions = builder.ApplicationServices.GetRequiredService<IConfiguration>().GetSection(Redis).Get<RedisCacheOptions>();
        if (redisOptions?.UseRedis == true)
        {
            builder.UseRedisCaching();
        }
    }
}
