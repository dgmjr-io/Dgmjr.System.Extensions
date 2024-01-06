namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Dgmjr.Redis;

public static partial class RedisWebApplicationBuilderExtensions
{
    private const string Redis = nameof(Redis);
    private const string ResponseCaching = nameof(ResponseCaching);

#if NET5_0_OR_GREATER
    public static IHostApplicationBuilder AddRedisCachingWithConnectionString(this IHostApplicationBuilder builder, string redisConnectionStringSectionName = Redis, string responseCachingConfigurationSectionName = ResponseCaching)
    {
        builder.Services.AddRedisCachingWithConnectionString(builder.Configuration, redisConnectionStringSectionName, responseCachingConfigurationSectionName);
        return builder;
    }
    public static IHostApplicationBuilder AddRedisCaching(this IHostApplicationBuilder builder, string redisConfigurationSectionName = Redis, string responseCachingConfigurationSectionName = ResponseCaching)
    {
        builder.Services.AddRedisCaching(builder.Configuration, redisConfigurationSectionName, responseCachingConfigurationSectionName);
        return builder;
    }

    public static IHostApplicationBuilder AddRedisCertificateLoader(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRedisCertificateLoader();
        return builder;
    }
#endif


    public static IServiceCollection AddRedisCertificateLoader(this IServiceCollection services)
    {
        services.AddSingleton<IPostConfigureOptions<RedisCacheOptions>, RedisCertificateLoaderAndValidator>();
        return services;
    }


    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration, string redisConfigurationSectionName = Redis, string responseCachingConfigurationSectionName = ResponseCaching)
    {
        var redisConfig = configuration.GetSection(redisConfigurationSectionName);
        services.AddSingleton<IConfigureOptions<RedisCacheOptions>>(new RedisEndpointCollectionProvider(redisConfig));
        services.AddStackExchangeRedisCache(options => redisConfig.Bind(options));
        services.AddResponseCaching(options => configuration.Bind(responseCachingConfigurationSectionName, options));
        return services;
    }

    public static IServiceCollection AddRedisCachingWithConnectionString(this IServiceCollection services, IConfiguration configuration, string redisConnectionStringSectionName = Redis, string responseCachingConfigurationSectionName = ResponseCaching)
    {
        services.AddStackExchangeRedisCache(
            options => options.Configuration = configuration.GetConnectionString(redisConnectionStringSectionName)
        );
        services.AddResponseCaching(options => configuration.Bind(responseCachingConfigurationSectionName, options));
        return services;
    }

    public static IApplicationBuilder UseRedisCaching(this IApplicationBuilder app)
    {
        app.UseResponseCaching();
        return app;
    }

}
