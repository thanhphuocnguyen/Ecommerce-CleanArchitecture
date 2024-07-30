using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Contracts;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}