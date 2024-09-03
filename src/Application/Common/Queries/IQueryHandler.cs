using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Application.Common.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}