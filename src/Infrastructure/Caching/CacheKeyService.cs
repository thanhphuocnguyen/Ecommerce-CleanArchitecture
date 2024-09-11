using Ecommerce.Domain.Common.Interfaces;

namespace Ecommerce.Infrastructure.Caching;

public class CacheKeyService : ICacheKeyService
{
    public string GetCacheKey(string key, Guid userId)
    {
        return $"{key}_{userId}";
    }

    public string GetKeyForRequest<TRequest>(TRequest request)
    {
        return typeof(TRequest).Name;
    }
}