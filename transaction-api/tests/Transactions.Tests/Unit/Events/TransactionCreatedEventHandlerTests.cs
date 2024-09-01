using Moq;
using Transactions.Application.Events.TransactionCreated;
using Transactions.Domain.Entities;
using Transactions.Infrastructure.Core.Messaging;

namespace Transactions.Tests.Unit.Events;

public class TransactionCreatedEventHandlerTests
{
    private readonly Mock<IMessagingProvider> _messagingProviderMock;
    private readonly TransactionCreatedEventHandler _handler;

    public TransactionCreatedEventHandlerTests()
    {
        _messagingProviderMock = new Mock<IMessagingProvider>();
        _handler = new TransactionCreatedEventHandler(_messagingProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEventIsCreated_ShouldProduceMessageToKafka()
    {
        // Arrange
        var transactionEvent = new TransactionCreatedEvent(
            Guid.NewGuid(),
            DateTime.Now,
            100.0m,
            TransactionType.Credit,
            "Test Transaction"
        );

        // Act
        await _handler.Handle(transactionEvent, CancellationToken.None);

        // Assert
        _messagingProviderMock.Verify(m => m.ProduceAsync(It.Is<string>(s => s == "app.transactions.created"),
            It.IsAny<TransactionCreatedEvent>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProduceFails_ShouldThrowException()
    {
        // Arrange
        var transactionEvent = new TransactionCreatedEvent(
            Guid.NewGuid(),
            DateTime.Now,
            100.0m,
            TransactionType.Credit,
            "Test Transaction"
        );

        _messagingProviderMock.Setup(m => m.ProduceAsync(It.IsAny<string>(), It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Failed to produce message"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(transactionEvent, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldProduceMessageWithCorrectData()
    {
        // Arrange
        var transactionEvent = new TransactionCreatedEvent(
            Guid.NewGuid(),
            DateTime.Now,
            100.0m,
            TransactionType.Debit,
            "Sample Transaction"
        );

        // Act
        await _handler.Handle(transactionEvent, CancellationToken.None);

        // Assert
        _messagingProviderMock.Verify(m => m.ProduceAsync(
            It.Is<string>(s => s == "app.transactions.created"),
            It.Is<TransactionCreatedEvent>(e => e.Id == transactionEvent.Id &&
                                                e.Date == transactionEvent.Date &&
                                                e.Total == transactionEvent.Total &&
                                                e.Type == transactionEvent.Type &&
                                                e.Description == transactionEvent.Description),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}