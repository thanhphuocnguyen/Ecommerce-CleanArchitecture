using Ecommerce.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Infrastructure;

public class UserContextMiddleware(IUserContextInitializer userContextInitializer) : IMiddleware
{
    private readonly IUserContextInitializer _userContextInitializer = userContextInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _userContextInitializer.SetCurrentUser(context.User);

        await next(context);
    }
}