namespace Ecommerce.Domain.Identity.Roles;

public record UpdateRolePermissionsRequest(Guid Id, List<string> Permissions);