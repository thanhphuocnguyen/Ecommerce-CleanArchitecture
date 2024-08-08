namespace Ecommerce.Application.Common.Interfaces;

public interface IJwtProvider
{
    Task<string> CreateTokenAsync();
}