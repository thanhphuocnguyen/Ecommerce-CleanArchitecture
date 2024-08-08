namespace Ecommerce.Application.Identity.Users.Contracts;

public class ResetPasswordRequest
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ConfirmPassword { get; set; } = default!;
}