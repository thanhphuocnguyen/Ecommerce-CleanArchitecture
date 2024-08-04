using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Roles;

public record RemoveRoleCommand(UserId UserId, int RoleId) : ICommand;