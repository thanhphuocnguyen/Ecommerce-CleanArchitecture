using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Entities;

public class AppUser : IdentityUser
{
    [NotMapped]
    public new UserId Id
    {
        get => new UserId(Guid.Parse(base.Id));
        private set => base.Id = value.Value.ToString();
    }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }
}