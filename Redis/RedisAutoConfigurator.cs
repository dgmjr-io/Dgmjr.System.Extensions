namespace Microsoft.Extensions.DependencyInjection;

using Dgmjr.Configuration.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

public class RedisAutoConfigurator : ConfiguratorBase<RedisAutoConfigurator>
{
    private const string Redis = nameof(Redis);
    private const string ResponseCaching = nameof(ResponseCaching);
    public ConfigurationOrder Order => ConfigurationOrder.AnyTime;

    protected override void ConfigureServices(IServiceCollection services)
    {
        var redisOptions = Configuration.GetSection(Redis).Get<RedisCacheOptions>();
        if (redisOptions?.UseRedis == true)
        {
            services.AddRedisCaching(Configuration);
        }
    }

    protected override void Configure(IApplicationBuilder app)
    {
        var redisOptions = app.ApplicationServices
            .GetRequiredService<IConfiguration>()
            .GetSection(Redis)
            .Get<RedisCacheOptions>();
        if (redisOptions?.UseRedis == true)
        {
            app.UseRedisCaching();
        }
    }
}
