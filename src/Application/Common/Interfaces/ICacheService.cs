namespace Ecommerce.Domain.Common.Interfaces;

public interface ICacheService
{
    Task<T> GetOrAddAsync<T>(
        string key,
        Func<CancellationToken?, Task<T>> factory,
        TimeSpan? slidingExpiration = null,
        CancellationToken cancellationToken = default);

    Task<T> GetOrAddAsync<T>(string key, Func<CancellationToken?, Task<T>> factory, CancellationToken cancellationToken);

    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken);

    Task<T> GetOrAddAsync<T>(string key, Func<T> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    Task<T> GetOrAddAsync<T>(string key, Func<T> factory, CancellationToken cancellationToken);

    Task<T> GetOrAddAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    Task<T> GetOrAddAsync<T>(string key, T value, CancellationToken cancellationToken);

    Task RemoveAsync(string key, CancellationToken cancellationToken);

    Task RemoveAllAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken);

    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
}