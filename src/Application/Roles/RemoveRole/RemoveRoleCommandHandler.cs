using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Repositories;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Application.Roles;

public class RemoveRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : ICommandHandler<RemoveRoleCommand>
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role is null)
        {
            return Result.Failure(DomainErrors.User.UserNotFound);
        }

        if (!role.Users.Any(x => x.Id == request.UserId))
        {
            return Result.Failure(DomainErrors.Role.RoleIsNotAssignedToUser);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}