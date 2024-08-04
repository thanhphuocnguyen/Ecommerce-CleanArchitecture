using Carter;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Users;
using Ecommerce.WebAPI.Exceptions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebAPI.Controllers;

public class AccountsEndpoints : ICarterModule
{
    public const string Login = "login";
    public const string Register = "register";
    public const string AccountInfo = "account-info";
    public const string AddRole = "add-role";
    public const string RemoveRole = "remove-role";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounts");

        group
            .MapPost(Login, LoginAccount)
            .WithName(nameof(LoginAccount));

        group
            .MapPost(Register, RegisterAccount)
            .WithName(nameof(RegisterAccount));

        group
            .MapGet(AccountInfo, GetAccountInfo)
            .RequireAuthorization()
            .WithName(nameof(GetAccountInfo));
    }

    private async Task<IResult> RegisterAccount(ISender sender, [FromBody] RegisterRequest request)
    {
        var result = await sender.Send(request.Adapt<RegisterCommand>());

        return result.Match(
            () => Results.Ok(result.Value));
    }

    private async Task<IResult> LoginAccount(ISender sender, LoginRequest request)
    {
        var result = await sender.Send(request.Adapt<LoginCommand>());

        return result.Match(
            () => Results.Ok(result.Value));
    }

    private async Task<IResult> GetAccountInfo(ISender sender, IUserContext userContext)
    {
        var result = await sender.Send(new GetUserQuery(userContext.UserId));

        return result.Match(
            () => Results.Ok(result.Value.Adapt<UserResponse>()));
    }
}