namespace Ecommerce.Infrastructure.Authentication.Jwt;

public class JwtOptions
{
    public string Secret { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public int ExpiryMinutes { get; set; }

    public int RefreshTokenExpiryMinutes { get; set; }
}