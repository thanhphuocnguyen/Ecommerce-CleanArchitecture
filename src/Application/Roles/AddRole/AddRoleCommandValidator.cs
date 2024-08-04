using Ecommerce.Application.Roles;
using FluentValidation;

namespace Ecommerce.Application;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}