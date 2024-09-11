namespace Ecommerce.Domain.Identity.Tokens;

public record TokenRequest
{
    public string Email { get; init; } = default!;

    public string Password { get; init; } = default!;
}