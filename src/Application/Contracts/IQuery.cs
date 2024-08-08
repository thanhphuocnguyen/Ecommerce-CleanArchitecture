using Ecommerce.Domain.Shared.Results;
using MediatR;

namespace Ecommerce.Application.Contracts;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}