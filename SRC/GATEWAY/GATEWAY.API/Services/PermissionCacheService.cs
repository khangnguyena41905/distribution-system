using System.Text.Json;
using GATEWAY.API.Dtos.PermissionDtos;
using StackExchange.Redis;

namespace GATEWAY.API.Services;

public interface IPermissionCacheService
{
    Task SetUserPermissionsAsync(Guid userId, List<FunctionDto> permissions, TimeSpan? expiry = null);
    Task<List<FunctionDto>?> GetUserPermissionsAsync(Guid userId);
    Task RemoveUserPermissionsAsync(Guid userId);
}

public class PermissionCacheService : IPermissionCacheService
{
    private readonly IDatabase _redisDb;
    private const string CacheKeyPrefix = "permissions:";

    public PermissionCacheService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task SetUserPermissionsAsync(Guid userId, List<FunctionDto> permissions, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(permissions);
        await _redisDb.StringSetAsync($"{CacheKeyPrefix}{userId}", json, expiry);
    }

    public async Task<List<FunctionDto>?> GetUserPermissionsAsync(Guid userId)
    {
        var json = await _redisDb.StringGetAsync($"{CacheKeyPrefix}{userId}");
        return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<List<FunctionDto>>(json);
    }

    public async Task RemoveUserPermissionsAsync(Guid userId)
    {
        await _redisDb.KeyDeleteAsync($"{CacheKeyPrefix}{userId}");
    }
}