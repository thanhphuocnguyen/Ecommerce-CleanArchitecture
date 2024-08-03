using Ecommerce.Application.Contracts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;

namespace Ecommerce.Application.Users;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
    private readonly IUserRepository _memberRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository memberRepository, IJwtProvider jwtProvider)
    {
        _memberRepository = memberRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user
        User? user = await _memberRepository.GetByUserNameAsync(request.Username, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure(DomainErrors.User.UserNotFound);
        }

        // Check password
        if (!PasswordManager.VerifyHashedPassword(user.PasswordHash, request.Password))
        {
            return Result<string>.Failure(DomainErrors.User.InvalidPassword);
        }

        // Generate token
        string token = await _jwtProvider.CreateTokenAsync(user);

        // Return token
        return token;
    }
}