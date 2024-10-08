using FluentValidation;

namespace Ecommerce.Domain.Identity.Users.Contracts;

public record ForgotPasswordRequest(string Email);

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}