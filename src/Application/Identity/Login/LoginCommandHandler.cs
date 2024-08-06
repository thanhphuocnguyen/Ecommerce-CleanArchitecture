using Ecommerce.Application.Contracts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Shared;

namespace Ecommerce.Application.Users;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IJwtProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }

    public Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user
        return Task.FromResult(Result.Success("token"));
    }
}