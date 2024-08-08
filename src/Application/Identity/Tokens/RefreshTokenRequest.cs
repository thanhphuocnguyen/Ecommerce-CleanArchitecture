namespace Ecommerce.Application.Identity.Tokens;

public record RefreshTokenRequest
{
    public string AccessToken { get; init; } = default!;

    public string RefreshToken { get; init; } = default!;
}