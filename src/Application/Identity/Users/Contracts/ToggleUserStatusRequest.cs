namespace Ecommerce.Application.Identity.Users.Contracts;

public record ToggleUserStatusRequest(Guid UserId, bool IsActive);