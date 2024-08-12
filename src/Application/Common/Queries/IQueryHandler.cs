using Ecommerce.Domain.Shared.Results;
using MediatR;

namespace Ecommerce.Application.Common.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}