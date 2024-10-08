﻿namespace Ecommerce.Domain.Common.Queries;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
{
}

public interface ICachedQuery
{
    string CacheKey { get; }

    TimeSpan? SlidingExpiration { get; }
}