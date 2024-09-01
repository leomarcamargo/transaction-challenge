using Consolidations.Domain.Entities;

namespace Consolidations.Worker.Messages;

public record TransactionCreatedMessage(Guid Id,
    DateTime Date,
    decimal Total,
    TransactionType Type,
    string Description);