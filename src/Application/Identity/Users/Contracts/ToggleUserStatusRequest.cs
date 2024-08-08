using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Identity.Users.Contracts;

public class ToggleUserStatusRequest
{
    public UserId? UserId { get; set; }

    public bool IsActive { get; set; }
}