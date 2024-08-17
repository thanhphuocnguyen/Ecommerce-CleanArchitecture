using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Entities;

public class AppRole : IdentityRole<Guid>
{
    public AppRole(string name, string? description = null)
    : base(name)
    {
        NormalizedName = name.ToUpperInvariant();
        Description = description;
    }

    public string? Description { get; set; }
}