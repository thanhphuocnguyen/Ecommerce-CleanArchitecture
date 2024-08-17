using Ecommerce.Application.Common.Mailing;
using Ecommerce.Application.Identity.Users.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a service for managing user passwords.
/// </summary>
internal partial class UserService
{
    /// <summary>
    /// Changes the password for a user asynchronously.
    /// </summary>
    /// <param name="request">The request containing the necessary information for changing the password.</param>
    /// <param name="userId">The ID of the user whose password needs to be changed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return ValidationResult.WithErrors(result.GetErrors());
        }

        return Result.Success();
    }

    /// <summary>
    /// Resets the password for a user asynchronously.
    /// </summary>
    /// <param name="request">The request containing the necessary information for resetting the password.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the new password.</returns>
    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());

        if (user == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        var token = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!token.Succeeded)
        {
            return ValidationResult<string>.WithErrors(token.GetErrors());
        }

        return Result<string>.Success("Password reset successfully.");
    }

    public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());

        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            return Result<string>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        const string route = "accounts/reset-password";
        var endpointUri = new Uri($"{origin}/{route}");

        string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "token", code);
        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            "Reset Password",
            "Please reset your password by clicking <a href=\"" + passwordResetUrl + "\">here</a>.");

        _jobService.Enqueue(() => _mailService.SendEmailAsync(mailRequest, CancellationToken.None));

        return Result<string>.Success("Password reset link sent successfully.");
    }
}