namespace Ecommerce.Application.Users;

public record RegisterRequest(
    string Email,
    string PhoneNumber,
    string Password,
    string Username,
    string FirstName,
    string LastName);