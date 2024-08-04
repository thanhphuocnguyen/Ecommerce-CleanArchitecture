using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Roles;

public record RemoveRoleRequest(Guid UserId, int RoleId);