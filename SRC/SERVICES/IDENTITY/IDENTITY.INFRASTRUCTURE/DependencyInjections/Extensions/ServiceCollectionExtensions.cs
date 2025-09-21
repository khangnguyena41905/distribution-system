using IDENTITY.APPLICATION.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IDENTITY.INFRASTRUCTURE.DependencyInjections.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
    public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOption =>
        {
            var connectionString = configuration.GetSection("Redis")["Configuration"];
            redisOption.Configuration = connectionString;
        });
        
        return services;
    }
}