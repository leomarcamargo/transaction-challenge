using MediatR;
using Transactions.Domain.Entities;

namespace Transactions.Application.Commands.CreateTransaction;

public record CreateTransactionCommand(DateTime Date,
    decimal Total,
    TransactionType Type,
    string Description
) : IRequest<CreateTransactionResult>;