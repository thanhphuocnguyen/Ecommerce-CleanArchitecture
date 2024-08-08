using System.Security.Claims;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Infrastructure.Authentication;

internal class UserContext : IUserContext, IUserContextInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private UserId? _userId = null;

    public UserId? GetUserId() => IsAuthenticated() ? _user?.GetUserId() : _userId;

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

        _userId = Guid.TryParse(userId, out var id) ? new UserId(id) : null;
    }

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (user != null)
        {
            throw new InvalidOperationException("Cannot set current user when user id is already set");
        }

        _user = user;
    }
}

internal static class ClaimsPrincipalExtensions
{
    public static UserId? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out var id) ? new UserId(id) : null;
    }

    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
    }
}