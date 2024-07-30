using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    UserId UserId { get; }
}