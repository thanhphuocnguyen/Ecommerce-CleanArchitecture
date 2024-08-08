using System.Text;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.WebUtilities;

namespace Ecommerce.Infrastructure.Identity.Users;

/// <summary>
/// Represents a user service.
/// </summary>
internal partial class UserService
{
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