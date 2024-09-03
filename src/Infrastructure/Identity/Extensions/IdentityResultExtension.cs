using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Extensions;

public static class IdentityResultExtension
{
    public static Error[] GetErrors(this IdentityResult result)
    {
        return result.Errors
            .Select(e => Error.InvalidValue("Identity Errors", e.Description))
            .ToArray();
    }
}