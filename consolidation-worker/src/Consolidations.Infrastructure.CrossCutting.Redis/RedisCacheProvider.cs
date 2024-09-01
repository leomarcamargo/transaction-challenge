using Consolidations.Infrastructure.Core.Cache;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consolidations.Infrastructure.CrossCutting.Redis;

public class RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer) : ICacheProvider
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!, _jsonSerializerOptions) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken)
    {
        var jsonValue = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        await _database.StringSetAsync(key, jsonValue, expiration);
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
    {
        return await _database.KeyExistsAsync(key);
    }
}
