using Ecommerce.Domain.Shared.Result;
using MediatR;

namespace Ecommerce.Domain.Common.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}