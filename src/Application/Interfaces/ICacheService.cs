namespace Ecommerce.Application.Interfaces;

public interface ICacheService
{
    Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken?, Task<T>> factory,
        TimeSpan? slidingExpiration = null,
        CancellationToken cancellationToken = default);
}