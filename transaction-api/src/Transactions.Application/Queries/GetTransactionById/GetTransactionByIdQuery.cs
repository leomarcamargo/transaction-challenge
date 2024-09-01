using MediatR;

namespace Transactions.Application.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<GetTransactionByIdResult?>;