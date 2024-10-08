using System.Security.Claims;
using Ecommerce.Domain.Common.Interfaces;
using Ecommerce.Infrastructure.Identity.Users;

namespace Ecommerce.Infrastructure.Authentication;

internal class UserContext : IUserContext, IUserContextInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private Guid? _userId = null;

    public Guid? GetUserId() => IsAuthenticated() ? _user?.GetUserId() : _userId;

    public string? GetUserEmail()
    {
        return IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;
    }

    public bool IsInRole(string role)
    {
        return _user?.IsInRole(role) ?? false;
    }

    public IEnumerable<Claim>? GetUserClaims()
    {
        return _user?.Claims;
    }

    public bool IsAuthenticated()
    {
        return _user?.Identity?.IsAuthenticated ?? false;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != null)
        {
            throw new InvalidOperationException("Cannot set current user id when user is already set");
        }

        _userId = Guid.TryParse(userId, out var id) ? id : null;
    }

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new InvalidOperationException("Cannot set current user when user id is already set");
        }

        _user = user;
    }
}

internal static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out var id) ? id : null;
    }

    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
    }
}