using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Users;

public record AddRoleCommand(UserId UserId, Role Role) : ITransactionalCommand;