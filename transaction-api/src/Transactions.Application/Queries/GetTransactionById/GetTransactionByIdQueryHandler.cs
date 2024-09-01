using AutoMapper;
using MediatR;
using Transactions.Domain.Repositories;

namespace Transactions.Application.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository,
    IMapper mapper)
    : IRequestHandler<GetTransactionByIdQuery, GetTransactionByIdResult?>
{
    public async Task<GetTransactionByIdResult?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        return transaction == null 
            ? null 
            : mapper.Map<GetTransactionByIdResult>(transaction);
    }
}