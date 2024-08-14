using System;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Infrastructure.Data.Specifications;

public class EntitiesByPaginationFilterSpec<T, TResult> : EntitiesByBaseFilterSpec<T, TResult>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
    : base(filter)
    {
        Query.PaginateBy(filter);
    }
}

public class EntitiesByPaginationFilterSpec<T> : EntitiesByBaseFilterSpec<T>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
    : base(filter)
    {
        Query.PaginateBy(filter);
    }
}