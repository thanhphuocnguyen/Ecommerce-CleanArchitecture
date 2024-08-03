using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain;

public class UserRole
{
    public UserId UserId { get; set; } = null!;

    public User User { get; set; } = default!;

    public int RoleId { get; set; }

    public Role Role { get; set; } = default!;
}