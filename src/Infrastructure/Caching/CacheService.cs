using Ecommerce.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.Infrastructure.Caching;

public class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultSlidingExpiration = TimeSpan.FromMinutes(7);
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(key, out _);
        return Task.FromResult(true);
    }

    public Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<CancellationToken?, Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        T? result = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(slidingExpiration ?? DefaultSlidingExpiration);
                return factory(cancellationToken);
            });

        return result!;
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<CancellationToken?, Task<T>> factory, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<T> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, Func<T> factory, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetOrAddAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}