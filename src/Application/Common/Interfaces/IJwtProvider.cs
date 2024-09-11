namespace Ecommerce.Domain.Common.Interfaces;

public interface IJwtProvider
{
    Task<string> CreateTokenAsync();
}