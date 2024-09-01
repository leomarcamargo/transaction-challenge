namespace Transactions.Infrastructure.Core.Messaging;

public interface IMessagingProvider
{
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken);
}