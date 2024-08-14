using Ardalis.Specification;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Infrastructure.Data.Specifications;

public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) => Query.SearchBy(filter);
}

public class EntitiesByBaseFilterSpec<T> : Specification<T>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) => Query.SearchBy(filter);
}