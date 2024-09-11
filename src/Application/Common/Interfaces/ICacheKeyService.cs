namespace Ecommerce.Domain.Common.Interfaces;

public interface ICacheKeyService
{
    string GetKeyForRequest<TRequest>(TRequest request);

    string GetCacheKey(string key, Guid userId);
}