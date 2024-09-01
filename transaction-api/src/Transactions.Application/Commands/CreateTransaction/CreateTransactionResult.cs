using Transactions.Domain.Entities;

namespace Transactions.Application.Commands.CreateTransaction;

public record CreateTransactionResult(Guid Id,
    DateTime Date,
    decimal Total,
    TransactionType Type,
    string Description
);