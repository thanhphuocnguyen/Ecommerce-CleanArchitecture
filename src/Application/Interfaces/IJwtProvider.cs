namespace Ecommerce.Application.Interfaces;

public interface IJwtProvider
{
    Task<string> CreateTokenAsync();
}