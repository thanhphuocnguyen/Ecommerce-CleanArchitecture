using Carter;
using Ecommerce.Application;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Users;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Shared;
using Ecommerce.WebAPI.Exceptions;
using Mapster;
using MediatR;

namespace Ecommerce.WebAPI.Controllers;

public class AccountsEndpoints : ICarterModule
{
    public const string Login = "login";
    public const string Register = "register";
    public const string AccountInfo = "account-info";

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

    private static async Task<IResult> RegisterAccount(ISender sender, RegisterRequest request)
    {
        var result = await sender.Send(new RegisterCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Username,
            request.Password));

        return result.Match(
            () => Results.Ok(result.Value),
            failure => failure.ToProblemDetails());
    }

    private static async Task<IResult> LoginAccount(ISender sender, LoginRequest request)
    {
        var result = await sender.Send(new LoginCommand(
            request.Username,
            request.Password));

        return result.Match(
            () => Results.Ok(result.Value),
            failure => failure.ToProblemDetails());
    }

    private static async Task<IResult> GetAccountInfo(ISender sender, IUserContext userContext)
    {
        var result = await sender.Send(new GetUserQuery(userContext.UserId));

        return result.Match(
            () => Results.Ok(result.Value.Adapt<UserResponse>()),
            failure => failure.ToProblemDetails());
    }

    private static async Task<IResult> AddRole(ISender sender, AddRoleRequest request)
    {
        var role = Role.FromValue(request.Role);

        if (role is null)
        {
            return Results.Problem("Invalid role value", statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await sender.Send(new AddRoleCommand(new UserId(request.UserId), role));

        return result.Match(
            () => Results.Ok(),
            failure => failure.ToProblemDetails());
    }
}