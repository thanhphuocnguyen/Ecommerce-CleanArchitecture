using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain;

public class UserRole
{
    public UserId UserId { get; set; } = default!;

    public User User { get; set; } = default!;

    public int RoleId { get; set; }

    public Role Role { get; set; } = default!;
}