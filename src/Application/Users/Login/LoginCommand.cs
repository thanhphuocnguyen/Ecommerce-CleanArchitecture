using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Users;

public record LoginCommand(string Username, string Password) : ICommand<string>;