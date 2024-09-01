using Transactions.Infrastructure.Core.Abstractions;

namespace Transactions.Domain.Entities;

public class Transaction : Entity
{
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public string Description { get; set; }
}