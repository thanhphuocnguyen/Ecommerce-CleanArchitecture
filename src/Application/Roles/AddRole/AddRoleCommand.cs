using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Roles;

public record AddRoleCommand(UserId UserId, int RoleId) : ICommand;