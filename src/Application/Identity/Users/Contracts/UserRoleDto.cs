namespace Ecommerce.Application.Identity.Users.Contracts;

public record UserRoleDto(string RoleId, string RoleName, string? Description, bool Enabled);