namespace Transactions.Infrastructure.CrossCutting.Kafka;

public record KafkaOptions
{
    public string BootstrapServers { get; set; }
}