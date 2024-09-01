using Consolidations.Domain.Entities;
using Consolidations.Infrastructure.Core.Abstracions;

namespace Consolidations.Domain.Repositories;

public interface IConsolidatedTransactionRepository : IRepository<ConsolidatedTransaction>
{
    Task<decimal> GetSumOfTransactionsByDateAsync(DateTime date, CancellationToken cancellationToken);
}