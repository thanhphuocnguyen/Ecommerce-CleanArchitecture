using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Contracts;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}