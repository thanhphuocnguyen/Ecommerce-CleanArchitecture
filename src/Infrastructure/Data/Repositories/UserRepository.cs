using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories;

internal class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(User user)
    {
        _dbContext.Set<User>().Add(user);
    }

    public void Remove(User user)
    {
        _dbContext.Set<User>().Remove(user);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Update(User user)
    {
        _dbContext.Set<User>().Update(user);
    }

    public Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<User>().FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}