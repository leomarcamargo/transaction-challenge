using Consolidations.Domain.Entities;
using Consolidations.Domain.Repositories;
using Consolidations.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Consolidations.Infrastructure.Data.Repositories;

public class ConsolidatedTransactionRepository(ConsolidationDbContext context)
    : Repository<ConsolidatedTransaction>(context), IConsolidatedTransactionRepository
{
    private readonly ConsolidationDbContext _context = context;

    public async Task<decimal> GetSumOfTransactionsByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        return await _context.ConsolidatedTransactions
            .Where(ct => ct.Date.Date == date.Date)
            .SumAsync(ct => ct.Type == TransactionType.Credit ? ct.Total : -ct.Total, cancellationToken);
    }
}