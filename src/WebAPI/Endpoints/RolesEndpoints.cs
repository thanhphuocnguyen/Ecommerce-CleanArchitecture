using Carter;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Roles;
using Ecommerce.WebAPI.Exceptions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebAPI;

public class RolesEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("roles");

        group
            .MapPost("add-role", AddRole)
            .RequireAuthorization()
            .WithName(nameof(AddRole));

        group
            .MapPost("remove-role", RemoveRole)
            .RequireAuthorization()
            .WithName(nameof(RemoveRole));
    }

    private async Task<IResult> RemoveRole(ISender sender, [FromBody] RemoveRoleRequest request)
    {
        var result = await sender.Send(new RemoveRoleCommand(request.UserId, request.RoleId));

        return result.Match(
            () => Results.Ok());
    }

    private async Task<IResult> AddRole(ISender sender, IUserContext userContext, [FromBody] AddRoleRequest request)
    {
        var result = await sender.Send(request.Adapt<AddRoleCommand>());

        return result.Match(
            () => Results.Ok());
    }
}