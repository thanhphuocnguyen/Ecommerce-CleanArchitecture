namespace Ecommerce.Domain.Identity.Users.Contracts;

public record ResetPasswordRequest(string Email, string Token, string Password);