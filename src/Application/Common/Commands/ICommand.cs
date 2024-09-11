using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Domain.Common.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}