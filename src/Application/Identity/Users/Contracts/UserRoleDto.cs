namespace Ecommerce.Application.Identity.Users.Contracts;

public record UserRoleDto(Guid RoleId, string RoleName, string? Description, bool Enabled);