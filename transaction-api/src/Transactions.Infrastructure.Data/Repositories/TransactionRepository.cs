
using Microsoft.EntityFrameworkCore;
using Transactions.Domain.Entities;
using Transactions.Domain.Repositories;
using Transactions.Infrastructure.Data.Contexts;

namespace Transactions.Infrastructure.Data.Repositories;

public class TransactionRepository(TransactionDbContext context) : Repository<Transaction>(context), ITransactionRepository
{
    public async Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetPagedTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = context.Transactions;
        var totalCount = await query.CountAsync(cancellationToken);
        var transactions = await query
            .OrderByDescending(x => x.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (transactions, totalCount);
    }
}