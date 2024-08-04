using Ecommerce.Application.Contracts;

namespace Ecommerce.Application;

public record ModifyRoleCommand(int Role, Guid UserId) : ICommand;