using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

namespace Ecommerce.Application.Identity.Tokens;

public interface ITokenService
{
    Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request);

    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}