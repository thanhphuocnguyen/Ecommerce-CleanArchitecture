using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Mapster;

namespace Ecommerce.Application.Users;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, bool>
{
    public async Task<Result<bool>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(Result.Success(true));
    }
}