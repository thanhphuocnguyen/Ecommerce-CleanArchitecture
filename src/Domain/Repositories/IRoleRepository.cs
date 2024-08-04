using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    void Add(Role role);

    void Update(Role role);

    void Remove(Role role);
}