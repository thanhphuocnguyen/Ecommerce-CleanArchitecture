using Ecommerce.Domain.Primitives;

namespace Ecommerce.Domain.Entities;

public class Role : Enumeration<Role>
{
    public static readonly Role Admin = new Role(1, "Admin");

    public static readonly Role User = new Role(2, "User");

    public static readonly Role Shop = new Role(3, "Shop");

    public Role(int value, string name)
    : base(value, name)
    {
    }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public ICollection<User> Users { get; set; } = new List<User>();
}