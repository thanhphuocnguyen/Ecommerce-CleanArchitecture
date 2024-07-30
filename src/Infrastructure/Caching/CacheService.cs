using Ecommerce.Application.Interfaces;
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
}