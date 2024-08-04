using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;

namespace Ecommerce.Application.Roles;

public class RemoveRoleCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<RemoveRoleCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        var role = user.Roles.FirstOrDefault(x => x.Value == request.RoleId);

        if (role is null)
        {
            return Result.Failure(DomainErrors.Role.RoleNotFound);
        }

        user.RemoveRole(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}