using Ecommerce.Application.Contracts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Users;

namespace Ecommerce.Application.Users;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, RegisterResponseDto>
{
    public async Task<Result<RegisterResponseDto>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = await userRepository.GetByEmailAsync(request.Email);

        if (email is not null)
        {
            return Result<RegisterResponseDto>.Failure(DomainErrors.User.UserAlreadyExists);
        }

        var passwordHashed = PasswordManager.HashPassword(request.Password);

        var user = User.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            username: request.Username,
            password: passwordHashed);

        if (user.IsFailure)
        {
            return Result<RegisterResponseDto>.Failure(user.Error);
        }

        userRepository.Add(user.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = await jwtProvider.CreateTokenAsync(user.Value);

        return Result<RegisterResponseDto>.Success(
            new RegisterResponseDto(
                token,
                user.Value.Email));
    }
}