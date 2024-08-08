namespace Ecommerce.Application.Identity.Roles;

public record CreateOrUpdateRoleRequest
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}