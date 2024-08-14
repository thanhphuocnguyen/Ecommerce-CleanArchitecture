using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Common.Interfaces;

public interface ICacheKeyService
{
    string GetKeyForRequest<TRequest>(TRequest request);

    string GetCacheKey(string key, UserId userId);
}