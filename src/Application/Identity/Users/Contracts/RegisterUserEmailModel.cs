namespace Ecommerce.Domain.Identity.Users.Contracts;

public record RegisterUserEmailModel(string Email, string UserName, string VerificationUri);