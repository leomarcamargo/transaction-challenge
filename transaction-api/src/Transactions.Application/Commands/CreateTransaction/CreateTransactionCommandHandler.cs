using AutoMapper;
using MediatR;
using Transactions.Application.Events.TransactionCreated;
using Transactions.Domain.Entities;
using Transactions.Domain.Repositories;

namespace Transactions.Application.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IMediator mediator,
    IMapper mapper)
    : IRequestHandler<CreateTransactionCommand, CreateTransactionResult>
{
    public async Task<CreateTransactionResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = mapper.Map<Transaction>(request);
        var createdTransaction = await transactionRepository.AddAsync(transaction, cancellationToken);

        var transactionCreatedEvent = mapper.Map<TransactionCreatedEvent>(createdTransaction);
        await mediator.Publish(transactionCreatedEvent, cancellationToken);

        return mapper.Map<CreateTransactionResult>(createdTransaction);
    }
}