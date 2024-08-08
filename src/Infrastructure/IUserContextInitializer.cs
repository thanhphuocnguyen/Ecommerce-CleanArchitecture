using System.Security.Claims;

namespace Ecommerce.Infrastructure;

public interface IUserContextInitializer
{
    void SetCurrentUserId(string userId);

    void SetCurrentUser(ClaimsPrincipal user);
}