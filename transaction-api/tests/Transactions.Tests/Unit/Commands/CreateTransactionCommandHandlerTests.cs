using AutoMapper;
using FluentValidation;
using MediatR;
using Moq;
using Transactions.Application.Behaviors;
using Transactions.Application.Commands.CreateTransaction;
using Transactions.Application.Events.TransactionCreated;
using Transactions.Domain.Entities;
using Transactions.Domain.Repositories;
using Transactions.Tests.Core;

namespace Transactions.Tests.Unit.Commands;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IMapper _mapper;
    private readonly IRequestHandler<CreateTransactionCommand, CreateTransactionResult> _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _mediatorMock = new Mock<IMediator>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateTransactionCommand, Transaction>();
            cfg.CreateMap<Transaction, TransactionCreatedEvent>();
            cfg.CreateMap<Transaction, CreateTransactionResult>();
        });
        _mapper = config.CreateMapper();

        var validators = new IValidator<CreateTransactionCommand>[]
        {
            new CreateTransactionCommandValidator()
        };

        var actualHandler = new CreateTransactionCommandHandler(
            _transactionRepositoryMock.Object,
            _mediatorMock.Object,
            _mapper);

        var validationBehavior = new ValidationBehavior<CreateTransactionCommand, CreateTransactionResult>(validators);
        _handler = new PipelineBehaviorDecorator<CreateTransactionCommand, CreateTransactionResult>(
            validationBehavior, actualHandler);
    }

    [Fact]
    public async Task Handle_WhenTransactionIsValid_ShouldAddTransactionAndPublishEvent()
    {
        // Arrange
        var command = new CreateTransactionCommand(Date: DateTime.Now,
            Total: 100.0m,
            Type: TransactionType.Credit,
            Description: "Test Transaction");

        var transaction = _mapper.Map<Transaction>(command);
        _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(command.Total, result.Total);
        Assert.Equal(command.Type, result.Type);
        Assert.Equal(command.Description, result.Description);
    }

    [Fact]
    public async Task Handle_WhenTransactionTotalIsNegative_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            Date: DateTime.Now,
            Total: -100.0m,  // Valor negativo para simular erro
            Type: TransactionType.Credit,
            Description: "Invalid Transaction"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

        _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Never);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRepositoryAddFails_ShouldNotPublishEvent()
    {
        // Arrange
        var command = new CreateTransactionCommand(
            Date: DateTime.Now,
            Total: 100.0m,
            Type: TransactionType.Debit,
            Description: "Test Transaction"
        );

        _transactionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        _mediatorMock.Verify(m => m.Publish(It.IsAny<TransactionCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}