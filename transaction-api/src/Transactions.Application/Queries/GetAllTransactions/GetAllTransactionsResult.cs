using Transactions.Application.DTOs;

namespace Transactions.Application.Queries.GetAllTransactions;

public record GetAllTransactionsResult(
    IEnumerable<TransactionDto> Transactions,
    int TotalCount,
    int PageNumber,
    int PageSize);