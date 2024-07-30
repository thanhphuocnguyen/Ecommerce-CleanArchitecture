using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Users;

public record LoginCommand(string Email, string Password) : ICommand<string>
{
}