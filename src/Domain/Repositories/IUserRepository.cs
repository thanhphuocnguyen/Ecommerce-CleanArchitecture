using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;

namespace Ecommerce.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

    void Add(User user);

    void Update(User user);

    void Remove(User user);
}