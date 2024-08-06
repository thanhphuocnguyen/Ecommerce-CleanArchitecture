using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Application.Users;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    public Task<Result> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Success());
    }
}