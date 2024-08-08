using Carter;

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

        // group
        //     .MapPost(Login, LoginAccount)
        //     .WithName(nameof(LoginAccount));

        // group
        //     .MapPost(Register, RegisterAccount)
        //     .WithName(nameof(RegisterAccount));

        // group
        //     .MapGet(AccountInfo, GetAccountInfo)
        //     .RequireAuthorization()
        //     .WithName(nameof(GetAccountInfo));
    }
}