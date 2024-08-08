using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Contracts;
using MediatR;

namespace Ecommerce.Application.Behaviors;

internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>(
    ICacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await _cacheService.GetOrAddAsync(
            request.CacheKey,
            (cancellationToken) => next(),
            request.SlidingExpiration,
            cancellationToken);
    }
}