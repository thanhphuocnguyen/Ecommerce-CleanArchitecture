namespace Ecommerce.Application.Identity.Users.Contracts;

public record RegisterUserEmailModel(string Email, string UserName, string VerificationUri);