using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Domain.Common.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}