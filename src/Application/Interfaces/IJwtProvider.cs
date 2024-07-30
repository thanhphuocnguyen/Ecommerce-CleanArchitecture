using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IJwtProvider
{
    Task<string> CreateTokenAsync(User user);
}