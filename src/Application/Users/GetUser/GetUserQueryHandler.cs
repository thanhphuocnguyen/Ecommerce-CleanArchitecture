using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;
using Mapster;

namespace Ecommerce.Application.Users;

public class GetUserQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(DomainErrors.User.UserNotFound);
        }

        var response = new UserResponse(
            user.Id.Value,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.Addresses.Select(x => new AddressResponse(
                x.Street,
                x.City,
                x.State,
                x?.Country,
                x?.ZipCode)).ToList(),
            user.Roles.Select(role => new RoleResponse(role.Value, role.Name, role.Permissions.Select(x => new PermissionResponse(x.Id, x.Name)).ToList())).ToList(),
            user.Created,
            user?.LastModified);

        return response;
    }
}