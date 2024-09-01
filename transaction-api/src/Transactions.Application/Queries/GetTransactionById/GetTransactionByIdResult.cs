namespace Transactions.Application.Queries.GetTransactionById;

public record GetTransactionByIdResult(Guid Id,
    DateTime Date,
    decimal Total,
    string Type,
    string Description);