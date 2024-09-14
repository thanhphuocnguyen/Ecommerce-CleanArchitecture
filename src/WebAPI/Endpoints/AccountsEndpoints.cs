using Carter;
using Ecommerce.Domain.Common.Interfaces;
using Ecommerce.Domain.Identity.Interface;
using Ecommerce.Domain.Identity.Tokens;
using Ecommerce.Domain.Identity.Users.Contracts;
using Ecommerce.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebAPI.Controllers;

public class AccountsEndpoints : ICarterModule
{
    public const string LoginPath = "login";
    public const string RegisterPath = "register";
    public const string AccountInfoPath = "account-info";
    public const string AddRolePath = "add-role";
    public const string RemoveRolePath = "remove-role";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounts");

        group
            .MapPost(LoginPath, Login)
            .WithName("Account Login");

        group
            .MapPost(RegisterPath, Register)
            .WithName("Account Register");

        group
            .MapGet(AccountInfoPath, GetAccountInfo)
            .RequireAuthorization()
            .WithName("Account Info");
    }

    private async Task<IResult> Register([FromBody] CreateUserRequest request, IUserService userService, HttpRequest httpRequest)
    {
        var result = await userService.CreateAsync(request, GetOriginFromRequest(httpRequest));

        return result.Match(() => Results.Ok(result.Value));
    }

    private async Task<IResult> Login([FromBody] TokenRequest tokenRequest, ITokenService tokenService)
    {
        var result = await tokenService.GetTokenAsync(tokenRequest);

        return result.Match(() => Results.Ok(result.Value));
    }

    private async Task<IResult> GetAccountInfo(IUserContext userCtx, IUserService userService)
    {
        var userId = userCtx.GetUserId();
        if (!userId.HasValue)
        {
            return Results.Unauthorized();
        }

        var result = await userService.GetAsync(userId.Value, default!);

        return result.Match(() => Results.Ok(result.Value));
    }

    private string GetOriginFromRequest(HttpRequest request) => $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}";
}