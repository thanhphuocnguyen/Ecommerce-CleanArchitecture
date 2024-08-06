using System.Security.Claims;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Identity;

internal class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public UserId UserId => httpContextAccessor?.HttpContext?.User?.GetUserId() ?? throw new ApplicationException("User id is unavailable");

    public bool IsAuthenticated => httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? throw new ApplicationException("User context is unavailable");
}

internal static class ClaimsPrincipalExtensions
{
    public static UserId GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            new(parsedUserId) :
            throw new ApplicationException("User id is unavailable");
    }
}