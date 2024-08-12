using Ecommerce.Domain.Shared.Results;
using MediatR;

namespace Ecommerce.Application.Common.Commands;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}