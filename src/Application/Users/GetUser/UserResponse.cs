using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Users;

public record UserResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        List<Address> Addresses,
        List<string> Role,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt)
{
}