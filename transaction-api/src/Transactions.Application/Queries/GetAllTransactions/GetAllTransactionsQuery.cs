using MediatR;

namespace Transactions.Application.Queries.GetAllTransactions;

public record GetAllTransactionsQuery(int PageNumber, int PageSize) : IRequest<GetAllTransactionsResult>;