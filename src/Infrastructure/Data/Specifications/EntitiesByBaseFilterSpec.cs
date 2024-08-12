using Ardalis.Specification;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Infrastructure.Data.Specifications;

public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    protected EntitiesByBaseFilterSpec(BaseFilter filter) => Query.SearchBy(filter);
}