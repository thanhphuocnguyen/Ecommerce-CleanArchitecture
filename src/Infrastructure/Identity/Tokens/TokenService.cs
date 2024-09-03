using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Application.Identity.Tokens;
using Ecommerce.Domain.Errors;
using Ecommerce.Domain.Shared.Result;
using Ecommerce.Infrastructure.Authentication;
using Ecommerce.Infrastructure.Authentication.Jwt;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Identity.Tokens;

internal class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _userManager = userManager;
    }

    public async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not AppUser user || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.InvalidEmailOrPassword);
        }

        if (!user.IsActive)
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.UserIsNotActive);
        }

        if (!user.EmailConfirmed)
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.EmailNotConfirmed);
        }

        return await GenerateTokensAndUpdateUser(user);
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (userPrincipal.IsFailure)
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.InvalidToken);
        }

        string? userEmail = userPrincipal.Value.GetEmail();
        var user = await _userManager.FindByEmailAsync(userEmail!);

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.InvalidEmailOrPassword);
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Failure<TokenResponse>(DomainErrors.Identity.Token.InvalidRefreshToken);
        }

        var result = GenerateTokensAndUpdateUser(user).Result;

        return result;
    }

    private Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(object token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            ValidateLifetime = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token.ToString(), tokenValidationParameters, out var securityToken);
        if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return Result.Failure<ClaimsPrincipal>(DomainErrors.Identity.Token.InvalidToken);
        }

        if (principal == null)
        {
            return Result.Failure<ClaimsPrincipal>(DomainErrors.Identity.Token.InvalidToken);
        }

        return principal;
    }

    private async Task<Result<TokenResponse>> GenerateTokensAndUpdateUser(AppUser user)
    {
        string token = GenerateJwt(user);
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(_options.RefreshTokenExpiryMinutes);
        await _userManager.UpdateAsync(user);
        return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwt(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _options.Audience),
            new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(_options.ExpiryMinutes).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };

        byte[] secret = Encoding.UTF8.GetBytes(_options.Secret);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}