using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure;

public class AppRole : IdentityRole
{
    public AppRole(string name, string? description = null)
    : base(name)
    {
        NormalizedName = name.ToUpperInvariant();
        Description = description;
    }

    public string? Description { get; set; }
}