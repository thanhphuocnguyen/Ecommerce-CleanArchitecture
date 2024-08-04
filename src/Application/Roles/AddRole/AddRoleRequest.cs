using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Roles;

public record AddRoleRequest(string RoleName, UserId UserId);