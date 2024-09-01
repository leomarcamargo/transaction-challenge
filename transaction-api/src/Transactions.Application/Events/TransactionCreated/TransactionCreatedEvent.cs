using MediatR;
using Transactions.Domain.Entities;

namespace Transactions.Application.Events.TransactionCreated;

public record TransactionCreatedEvent : INotification
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public decimal Total { get; init; }
    public TransactionType Type { get; init; }
    public string Description { get; init; }

    public TransactionCreatedEvent() { }

    public TransactionCreatedEvent(Guid id, DateTime date, decimal total, TransactionType type, string description)
    {
        Id = id;
        Date = date;
        Total = total;
        Type = type;
        Description = description;
    }
}