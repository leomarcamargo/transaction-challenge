using Transactions.Domain.Entities;

namespace Transactions.Application.DTOs;

public record TransactionDto(Guid Id,
    decimal Total,
    DateTime Date,
    TransactionType Type,
    string Description);