using MediatR;

namespace Transactions.Tests.Core;

public class PipelineBehaviorDecorator<TRequest, TResponse>(
    IPipelineBehavior<TRequest, TResponse> behavior,
    IRequestHandler<TRequest, TResponse> inner)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return behavior.Handle(request, () => inner.Handle(request, cancellationToken), cancellationToken);
    }
}