using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.Domain.Identity.Tokens;

public interface ITokenService
{
    Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request);

    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}