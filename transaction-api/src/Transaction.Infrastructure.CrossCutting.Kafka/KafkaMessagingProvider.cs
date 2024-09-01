using System.Text.Json;
using Confluent.Kafka;
using Transactions.Infrastructure.Core.Messaging;

namespace Transactions.Infrastructure.CrossCutting.Kafka;

public class KafkaMessagingProvider(IProducer<string, string> producer)
    : IMessagingProvider
{
    public async Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken)
    {
        var serializedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var kafkaMessage = new Message<string, string>
        {
            Key = typeof(T).Name,
            Value = serializedMessage
        };

        await producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
    }
}