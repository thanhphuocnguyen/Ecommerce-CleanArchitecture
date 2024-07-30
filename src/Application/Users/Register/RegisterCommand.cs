using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Users;

public record RegisterCommand(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Username,
    string Password) : ICommand<RegisterResponseDto>
{
}