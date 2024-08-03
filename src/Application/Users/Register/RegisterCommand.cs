using Ecommerce.Application.Contracts;

namespace Ecommerce.Application.Users;

public record RegisterCommand(
    string Email,
    string Username,
    string Password,
    string PhoneNumber,
    string FirstName,
    string LastName) : ICommand<RegisterResponseDto>;