using Transactions.Domain.Entities;
using Transactions.Infrastructure.Core.Abstractions;

namespace Transactions.Domain.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<(IEnumerable<Transaction> Transactions, int TotalCount)> GetPagedTransactionsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}