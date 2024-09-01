using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Consolidations.Infrastructure.Core.Cache;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Consolidations.Infrastructure.CrossCutting.Redis;

public static class RedisServiceExtensions
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheProvider, RedisCacheProvider>();

        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisOptions>();
        if (redisSettings == null || string.IsNullOrEmpty(redisSettings.Host))
            throw new InvalidOperationException("Redis settings are not configured");

        services.Configure<RedisOptions>(configuration.GetSection("RedisSettings"));

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
            var connectionString = $"{settings.Host}:{settings.Port}";
            if (!string.IsNullOrEmpty(settings.Password))
                connectionString += $",password={settings.Password}";
            return ConnectionMultiplexer.Connect(connectionString);
        });

        return services;
    }
}