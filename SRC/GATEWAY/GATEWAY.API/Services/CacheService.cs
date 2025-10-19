using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace GATEWAY.API.Services;
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;

    Task SetAsync<T>(string key, T value, TimeSpan fromHours, CancellationToken cancellationToken = default)
        where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var data = await _distributedCache.GetStringAsync(key, cancellationToken);
        return string.IsNullOrWhiteSpace(data) ? null : JsonSerializer.Deserialize<T>(data, _serializerOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan fromHours,
        CancellationToken cancellationToken = default) where T : class
    {
        var data = JsonSerializer.Serialize(value, _serializerOptions);
        await _distributedCache.SetStringAsync(key, data, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Remove by prefix is not supported in IDistributedCache.");
    }
}