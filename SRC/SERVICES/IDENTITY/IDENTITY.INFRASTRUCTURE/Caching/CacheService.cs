using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using IDENTITY.APPLICATION.Abstractions;

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
        // ❌ Không hỗ trợ trực tiếp với IDistributedCache.
        // Cần kết hợp thêm Redis IServer như cũ nếu thật sự cần xóa theo prefix.
        // Hoặc duy trì danh sách key cache theo user để xóa.
        throw new NotSupportedException("Remove by prefix is not supported in IDistributedCache.");
    }
}