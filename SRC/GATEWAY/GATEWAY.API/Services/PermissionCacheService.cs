using System.Text.Json;
using GATEWAY.API.Dtos.PermissionDtos;
using StackExchange.Redis;

namespace GATEWAY.API.Services;

public interface IPermissionCacheService
{
    Task<List<FunctionDto>?> GetUserPermissionsAsync(Guid userId);
}

public class PermissionCacheService : IPermissionCacheService
{
    private readonly ICacheService _redisDb;
    private const string CacheKeyPrefix = "permissions:";

    public PermissionCacheService(CacheService redis)
    {
        _redisDb = redis;
    }
    
    public async Task<List<FunctionDto>?> GetUserPermissionsAsync(Guid userId)
    {
        var json = await _redisDb.GetAsync<List<FunctionDto>>($"{CacheKeyPrefix}{userId}");
        return json;
    }
}