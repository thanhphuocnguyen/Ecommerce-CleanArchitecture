using Ecommerce.Domain.Shared;
using MediatR;

namespace Ecommerce.Application.Contracts;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}