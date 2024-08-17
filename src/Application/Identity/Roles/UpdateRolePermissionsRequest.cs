namespace Ecommerce.Application.Identity.Roles;

public record UpdateRolePermissionsRequest(Guid Id, List<string> Permissions);