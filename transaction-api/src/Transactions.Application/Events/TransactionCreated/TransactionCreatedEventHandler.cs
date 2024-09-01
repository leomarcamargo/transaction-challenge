using MediatR;
using Transactions.Infrastructure.Core.Messaging;

namespace Transactions.Application.Events.TransactionCreated;

public class TransactionCreatedEventHandler(IMessagingProvider messagingProvider)
    : INotificationHandler<TransactionCreatedEvent>
{
    private const string TopicName = "app.transactions.created";

    public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        await messagingProvider.ProduceAsync(TopicName, notification, cancellationToken);
    }
}