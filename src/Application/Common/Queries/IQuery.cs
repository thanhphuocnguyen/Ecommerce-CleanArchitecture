using Ecommerce.Domain.Shared.Results;
using MediatR;

namespace Ecommerce.Application.Common.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}