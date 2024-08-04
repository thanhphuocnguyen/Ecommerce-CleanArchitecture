using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;

namespace Ecommerce.Application.Roles;

public class AddRoleCommandHandler(IRoleRepository roleRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddRoleCommand>
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        if (role.Users.Any(x => x.Id == request.UserId))
        {
            return Result.Failure(DomainErrors.User.RoleAlreadyExists);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        role.Users.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}