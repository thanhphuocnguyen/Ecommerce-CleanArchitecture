using FluentValidation;

namespace Ecommerce.Application.Roles;

public class RemoveRoleRequestValidator : AbstractValidator<RemoveRoleRequest>
{
    public RemoveRoleRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}