using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories;

public class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Add(Role role)
    {
        _dbContext.Add(role);
    }

    public Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles.Include(x => x.Users).FirstOrDefaultAsync(x => x.Value == id, cancellationToken);
    }

    public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public void Remove(Role role)
    {
        _dbContext.Remove(role);
    }

    public void Update(Role role)
    {
        _dbContext.Update(role);
    }
}