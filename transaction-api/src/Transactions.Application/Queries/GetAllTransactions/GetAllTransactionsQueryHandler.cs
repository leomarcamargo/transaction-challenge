using AutoMapper;
using MediatR;
using Transactions.Application.DTOs;
using Transactions.Domain.Repositories;

namespace Transactions.Application.Queries.GetAllTransactions;

public class GetAllTransactionsQueryHandler(
    ITransactionRepository transactionRepository,
    IMapper mapper)
    : IRequestHandler<GetAllTransactionsQuery, GetAllTransactionsResult>
{
    public async Task<GetAllTransactionsResult> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await transactionRepository.GetPagedTransactionsAsync(request.PageNumber, request.PageSize, cancellationToken);
        return new GetAllTransactionsResult(Transactions: mapper.Map<IEnumerable<TransactionDto>>(transactions.Transactions),
            TotalCount: transactions.TotalCount,
            PageNumber: request.PageNumber,
            PageSize: request.PageSize);
    }
}