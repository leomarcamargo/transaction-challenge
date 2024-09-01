using Consolidations.Infrastructure.Core.Abstracions;

namespace Consolidations.Domain.Entities;

public class ConsolidatedTransaction : Entity
{
    public Guid TransactionId { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set; }
    public decimal DailyBalance { get; set; }
    public string Description { get; set; }
}