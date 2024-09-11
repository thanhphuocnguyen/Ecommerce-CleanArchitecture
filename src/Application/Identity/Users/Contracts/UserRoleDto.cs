namespace Ecommerce.Domain.Identity.Users.Contracts;

public record UserRoleDto(Guid RoleId, string RoleName, string? Description, bool Enabled);