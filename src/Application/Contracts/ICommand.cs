using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;
using MediatR;

namespace Ecommerce.Application.Contracts;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}