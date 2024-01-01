namespace Microsoft.Extensions.DependencyInjection;
using Dgmjr.Redis.Extensions;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

public static class JsonConverterExtensions
{
    public static IServiceCollection AddRedisJson(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new EndPointCollectionJsonConverter()));
        return services;
    }
}
