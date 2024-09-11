namespace Ecommerce.Domain.Identity.Users.Contracts;

public record ToggleUserStatusRequest(Guid UserId, bool IsActive);