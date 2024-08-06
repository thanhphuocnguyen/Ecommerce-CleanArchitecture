using Ecommerce.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Ecommerce.Infrastructure.Identity;

public static class IdentityResultExtension
{
    public static Result GetErrors(this IdentityResult result, IStringLocalizer t)
    {
        var errors = result.Errors
            .Select(e => Error.InvalidValue("Identity Erros", t[e.Description].ToString()))
            .ToArray();
        return ValidationResult.WithErrors(errors);
    }
}