using System.Text;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared.Results;
using Ecommerce.Infrastructure.Identity.Entities;
using Ecommerce.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a user service.
/// </summary>
internal partial class UserService
{
    public async Task<Result<string>> ConfirmEmailAsync(UserId userId, string code, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == userId && !u.EmailConfirmed, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
        {
            return ValidationResult<string>.WithErrors(result.GetErrors());
        }

        return Result<string>.Success("Email confirmed successfully.");
    }

    public async Task<Result<string>> ConfirmChangePhoneNumberAsync(UserId userId, string code)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == userId && !u.PhoneNumberConfirmed);

        if (user == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        if (user.PhoneNumber == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.PhoneNumberNotProvided);
        }

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

        if (!result.Succeeded)
        {
            return ValidationResult<string>.WithErrors(result.GetErrors());
        }

        return Result<string>.Success("Phone number confirmed successfully.");
    }

    public async Task<Result<string>> ConfirmPhoneAsync(UserId userId, string code, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == userId && !u.PhoneNumberConfirmed, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.UserNotFound);
        }

        if (user.PhoneNumber == null)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.PhoneNumberNotProvided);
        }

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, code, user.PhoneNumber);

        if (!result)
        {
            return Result<string>.Failure(DomainErrors.Identity.User.InvalidPhoneNumberConfirmationCode);
        }

        return Result<string>.Success("Phone number confirmed successfully.");
    }

    private async Task<string> GetEmailVerificationUriAsync(AppUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email";
        var endpointUri = new Uri(string.Concat(origin, "/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id.ToString());
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "userId", user.Id.Value.ToString());
        return verificationUri;
    }
}