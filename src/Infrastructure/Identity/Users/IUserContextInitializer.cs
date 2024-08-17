using System.Security.Claims;

namespace Ecommerce.Infrastructure.Identity.Users;

public interface IUserContextInitializer
{
    void SetCurrentUserId(string userId);

    void SetCurrentUser(ClaimsPrincipal user);
}