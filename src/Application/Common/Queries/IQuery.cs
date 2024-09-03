using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Application.Common.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}