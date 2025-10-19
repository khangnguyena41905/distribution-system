using GATEWAY.API.Services;
using StackExchange.Redis;

namespace GATEWAY.API.DependencyInjections.Delegations;

public static class RedisServiceExtensions
{
    public static IServiceCollection AddGatewayService(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        // services.AddScoped<IPermissionCacheService, PermissionCacheService>();

        return services;
    }
    public static IServiceCollection AddRedisConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOption =>
        {
            var connectionString = configuration.GetSection("Redis")["Configuration"];
            redisOption.Configuration = connectionString;
        });
        
        return services;
    }
}
