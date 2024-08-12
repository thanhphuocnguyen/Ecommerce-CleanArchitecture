namespace Ecommerce.Infrastructure.Identity.Models;

public record RegisterUserEmailModel(string Email, string UserName, string VerificationUri);