using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;

namespace Ecommerce.Application.Users;

public class AddRoleCommandHandler(IUserRepository userRepository) : ICommandHandler<AddRoleCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failure(DomainErrors.User.UserNotFound);
        }

        user.AddRole(request.Role);

        _userRepository.Update(user);

        return Result.Success();
    }
}