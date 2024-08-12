namespace Ecommerce.Application.Identity.Users.Contracts;

public record ResetPasswordRequest(string Email, string Token, string Password);