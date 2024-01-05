namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public static partial class RedisWebApplicationBuilderExtensions
{
    private const string Redis = nameof(Redis);
    private const string ResponseCaching = nameof(ResponseCaching);

#if NET5_0_OR_GREATER
    public static IHostApplicationBuilder AddRedisCaching(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRedisCaching(builder.Configuration);
        return builder;
    }
#endif

    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(
            options => options.Configuration = configuration.GetConnectionString(Redis)
        );
        services.AddResponseCaching(options => configuration.Bind(ResponseCaching, options));
        return services;
    }

    public static IApplicationBuilder UseRedisCaching(this IApplicationBuilder app)
    {
        app.UseResponseCaching();
        return app;
    }
}
