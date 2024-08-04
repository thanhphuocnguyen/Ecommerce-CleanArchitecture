namespace Ecommerce.Application.Users;

public record UserResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        List<AddressResponse> Addresses,
        List<RoleResponse> Role,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt);

public record AddressResponse(
        string Street,
        string City,
        string State,
        string? Country,
        string? ZipCode);

public record PermissionResponse(
        int Id,
        string Name);

public record RoleResponse(
        int Id,
        string Name,
        List<PermissionResponse> Permissions);