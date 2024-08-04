using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Roles;

public record RemoveRoleRequest(UserId UserId, int RoleId);