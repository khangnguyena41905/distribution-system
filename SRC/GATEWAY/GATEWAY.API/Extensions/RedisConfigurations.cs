using GATEWAY.API.Services;
using StackExchange.Redis;

namespace GATEWAY.API.Extensions;

public class RedisConfigurations
{
    public IServiceCollection AddRedisConfigurations(IServiceCollection services)
    {
        // Redis configuration string
        var redisConnectionString = "localhost:6379"; 

        // Kết nối và đăng ký singleton
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        // Đăng ký service cache
        services.AddScoped<IPermissionCacheService, PermissionCacheService>();

        return services;
    }
}