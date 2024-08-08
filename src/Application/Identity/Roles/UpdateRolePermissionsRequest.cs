namespace Ecommerce.Application.Identity.Roles;

public record UpdateRolePermissionsRequest
{
    public string Id { get; set; } = default!;

    public List<string> Permissions { get; set; } = new();
}