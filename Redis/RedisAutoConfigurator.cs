namespace Microsoft.Extensions.DependencyInjection;

using Dgmjr.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

public class RedisAutoConfigurator : IHostingStartup, IStartupFilter
{
    private const string Redis = nameof(Redis);
    private const string ResponseCaching = nameof(ResponseCaching);
    public ConfigurationOrder Order => ConfigurationOrder.AnyTime;

    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            var redisOptions = context.Configuration.GetSection(Redis).Get<RedisCacheOptions>();
            if (redisOptions?.UseRedis == true)
            {
                services.AddRedisCaching(context.Configuration);
            }
        });
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            var redisOptions = builder.ApplicationServices
                .GetRequiredService<IConfiguration>()
                .GetSection(Redis)
                .Get<RedisCacheOptions>();
            if (redisOptions?.UseRedis == true)
            {
                builder.UseRedisCaching();
            }
            next(builder);
        };
    }
}
