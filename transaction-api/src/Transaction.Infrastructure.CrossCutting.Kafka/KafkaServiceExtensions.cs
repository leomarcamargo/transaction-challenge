using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Transactions.Infrastructure.Core.Messaging;

namespace Transactions.Infrastructure.CrossCutting.Kafka;

public static class KafkaServiceExtensions
{
    public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessagingProvider, KafkaMessagingProvider>();

        var kafkaSettings = configuration.GetSection("KafkaSettings").Get<KafkaOptions>();
        if (kafkaSettings == null || string.IsNullOrEmpty(kafkaSettings.BootstrapServers))
            throw new InvalidOperationException("Kafka settings are not configured");

        services.Configure<KafkaOptions>(configuration.GetSection("KafkaSettings"));

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers
            };

            return new ProducerBuilder<string, string>(config).Build();
        });

        return services;
    }
}
